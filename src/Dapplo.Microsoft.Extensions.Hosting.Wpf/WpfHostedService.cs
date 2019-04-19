// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dapplo.Microsoft.Extensions.Hosting.Wpf
{
    /// <summary>
    /// This hosts a WPF service, making sure the lifecycle is managed
    /// </summary>
    public class WpfHostedService : IHostedService
    {
        private readonly ILogger<WpfHostedService> _logger;
        private readonly IApplicationLifetime _applicationLifetime;
        private readonly IServiceProvider _serviceProvider;
        private readonly IWpfContext _wpfContext;

        /// <summary>
        /// The constructor which takes all the DI objects
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="applicationLifetime"></param>
        /// <param name="serviceProvider">IServiceProvider</param>
        /// <param name="wpfContext">IWpfContext</param>
        public WpfHostedService(ILogger<WpfHostedService> logger, IApplicationLifetime applicationLifetime, IServiceProvider serviceProvider, IWpfContext wpfContext)
        {
            _logger = logger;
            _applicationLifetime = applicationLifetime;
            _serviceProvider = serviceProvider;
            _wpfContext = wpfContext;
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            // Create a thread which runs WPF
            var newWpfThread = new Thread(WpfThreadStart)
            {
                IsBackground = true
            };
            // Set the apartment state
            newWpfThread.SetApartmentState(ApartmentState.STA);
            // Start the new WPF thread
            newWpfThread.Start(taskCompletionSource);
            
            return taskCompletionSource.Task;
        }

        /// <inheritdoc />
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_wpfContext.IsRunning)
            {
                _logger.LogDebug("Stopping WPF due to application exit.");
                // Stop application
                await _wpfContext.WpfApplication.Dispatcher.InvokeAsync(() => _wpfContext.WpfApplication.Shutdown());
            }
        }

        /// <summary>
        /// Start WPF
        /// </summary>
        private void WpfThreadStart(object taskCompletionSourceAsObject)
        {
            TaskCompletionSource<bool> taskCompletionSource = taskCompletionSourceAsObject as TaskCompletionSource<bool>;
            // Create our SynchronizationContext, and install it:
            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher));

            // Create the new WPF application
            var wpfApplication = new Application()
            {
                ShutdownMode = _wpfContext.ShutdownMode
            };

            // Register to the WPF application exit to stop the host application
            wpfApplication.Exit += (s,e) =>
            {
                _wpfContext.IsRunning = false;
                if (_wpfContext.IsLifetimeLinked)
                {
                    _logger.LogDebug("Stopping host application due to WPF application exit.");
                    _applicationLifetime.StopApplication();
                }
            };
            
            // Store the application for others to interact
            _wpfContext.WpfApplication = wpfApplication;

            // Mark the application as running
            _wpfContext.IsRunning = true;

            // Signal that we can continue
            taskCompletionSource?.SetResult(true);

            // Use the provided IWpfService
            var wpfServices = _serviceProvider.GetServices<IWpfService>();
            if (wpfServices != null)
            {
                foreach(var wpfService in wpfServices)
                {
                    wpfService.Initialize(wpfApplication);
                }
            }
            // Run the WPF application in this thread which was specifically created for it, with the specified shell
            if (_serviceProvider.GetService<IWpfShell>() is Window wpfShell)
            {
                wpfApplication.Run(wpfShell);
            }
            else
            {
                wpfApplication.Run();
            }
        }
    }
}
