//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2019 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Microsoft.Extensions.Hosting
// 
//  Dapplo.Microsoft.Extensions.Hosting is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Microsoft.Extensions.Hosting is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Microsoft.Extensions.Hosting. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

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
            // Create a thread which runs WPF
            var newWpfThread = new Thread(WpfThreadStart)
            {
                IsBackground = true
            };
            // Set the apartment state
            newWpfThread.SetApartmentState(ApartmentState.STA);
            // Start the new WPF thread
            newWpfThread.Start();
            
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (_wpfContext.IsRunning)
            {
                _logger.LogDebug("Stopping WPF due to application exit.");
                // Stop application
                _wpfContext.WpfApplication.Dispatcher.Invoke(() => _wpfContext.WpfApplication.Shutdown());
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Start WPF
        /// </summary>
        private void WpfThreadStart()
        {
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
