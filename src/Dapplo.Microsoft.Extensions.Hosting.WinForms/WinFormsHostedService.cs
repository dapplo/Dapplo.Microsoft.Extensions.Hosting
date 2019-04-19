// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dapplo.Microsoft.Extensions.Hosting.WinForms
{
    /// <summary>
    /// This hosts a WinForms service, making sure the lifecycle is managed
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class WinFormsHostedService : IHostedService
    {
        private readonly ILogger<WinFormsHostedService> _logger;
        private readonly IApplicationLifetime _applicationLifetime;
        private readonly IServiceProvider _serviceProvider;
        private readonly IWinFormsContext _winFormsContext;

        /// <summary>
        /// The constructor which takes all the DI objects
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="applicationLifetime">IApplicationLifetime</param>
        /// <param name="serviceProvider">IServiceProvider</param>
        /// <param name="winFormsContext">IWinFormsContext</param>
        public WinFormsHostedService(ILogger<WinFormsHostedService> logger, IApplicationLifetime applicationLifetime, IServiceProvider serviceProvider, IWinFormsContext winFormsContext)
        {
            _logger = logger;
            _applicationLifetime = applicationLifetime;
            _serviceProvider = serviceProvider;
            _winFormsContext = winFormsContext;
        }

        /// <inheritdoc />
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            // Create a thread which runs windows forms
            var newFormsThread = new Thread(FormsThreadStart)
            {
                IsBackground = true
            };
            // Set the apartment state
            newFormsThread.SetApartmentState(ApartmentState.STA);
            // Start the new Forms thread
            newFormsThread.Start(taskCompletionSource);
          
            await taskCompletionSource.Task;
        }

        /// <inheritdoc />
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_winFormsContext.IsRunning)
            {
                _logger.LogDebug("Stopping WinForms application.");
                await _winFormsContext.FormsDispatcher.InvokeAsync(()=>
                {
                    // Graceful close, otherwise finalizes try to dispose forms.
                    foreach (var form in Application.OpenForms.Cast<Form>().ToList())
                    {
                        try
                        {
                            form.Close();
                            form.Dispose();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Couldn't cleanup a Form");
                        }
                    }
                    Application.ExitThread();
                });
            }
        }
        
        /// <summary>
        /// Start Windows Forms
        /// </summary>
        private void FormsThreadStart(object taskCompletionSourceAsObject)
        {
            var taskCompletionSource = taskCompletionSourceAsObject as TaskCompletionSource<bool>;

            var currentDispatcher = Dispatcher.CurrentDispatcher;
            _winFormsContext.FormsDispatcher = currentDispatcher;

            // Create our SynchronizationContext, and install it:
            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(currentDispatcher));

            if (_winFormsContext.EnableVisualStyles)
            {
                Application.EnableVisualStyles();
            }

            // Register to the WinForms application exit to stop the host application
            Application.ApplicationExit += OnApplicationExit;

            // Signal that the startup is pretty much finished
            taskCompletionSource?.SetResult(true);

            // Run the application
            _winFormsContext.IsRunning = true;
            if (_serviceProvider.GetService<IWinFormsShell>() is Form formShell)
            {
                Application.Run(formShell);
            }
            else
            {
                Application.Run();
            }
            _logger.LogDebug("Windows Forms Application stopped.");
        }

        private void OnApplicationExit(object sender, EventArgs eventArgs)
        {
            Application.ApplicationExit -= OnApplicationExit;
            _winFormsContext.IsRunning = false;
            if (_winFormsContext.IsLifetimeLinked)
            {
                _logger.LogDebug("Stopping host application due to WinForms application exit.");
                _applicationLifetime.StopApplication();
            }
        }
    }
}
