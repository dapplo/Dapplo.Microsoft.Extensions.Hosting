// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Dapplo.Microsoft.Extensions.Hosting.UiThread;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace Dapplo.Microsoft.Extensions.Hosting.Wpf.Internals
{
    /// <summary>
    /// This contains the logic for the WPF thread
    /// </summary>
    public class WpfThread : BaseUiThread<IWpfContext>
    {
        /// <summary>
        /// This will create the WpfThread
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider</param>
        public WpfThread(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <inheritdoc />
        protected override void PreUiThreadStart()
        {
            // Create our SynchronizationContext, and install it:
            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher));

            // Create the new WPF application
            var wpfApplication = new Application()
            {
                ShutdownMode = _uiContext.ShutdownMode
            };

            // Register to the WPF application exit to stop the host application
            wpfApplication.Exit += (s, e) =>
            {
                _uiContext.IsRunning = false;
                if (_uiContext.IsLifetimeLinked)
                {
                    //_logger.LogDebug("Stopping host application due to WPF application exit.");
                    _serviceProvider.GetService<IHostApplicationLifetime>().StopApplication();
                }
            };

            // Store the application for others to interact
            _uiContext.WpfApplication = wpfApplication;
        }

        /// <inheritdoc />
        protected override void UiThreadStart() {
            // Mark the application as running
            _uiContext.IsRunning = true;

            // Use the provided IWpfService
            var wpfServices = _serviceProvider.GetServices<IWpfService>();
            if (wpfServices != null)
            {
                foreach(var wpfService in wpfServices)
                {
                    wpfService.Initialize(_uiContext.WpfApplication);
                }
            }
            // Run the WPF application in this thread which was specifically created for it, with the specified shell
            if (_serviceProvider.GetService<IWpfShell>() is Window wpfShell)
            {
                _uiContext.WpfApplication.Run(wpfShell);
            }
            else
            {
                _uiContext.WpfApplication.Run();
            }
        }
    }
}