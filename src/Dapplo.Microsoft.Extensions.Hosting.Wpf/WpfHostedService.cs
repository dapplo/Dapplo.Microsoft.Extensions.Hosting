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

using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
        private readonly IWpfContext _wpfContext;
        private readonly Window _shell;

        /// <summary>
        /// The constructor which takes all the DI objects
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="applicationLifetime"></param>
        /// <param name="wpfContext">IWpfContext</param>
        /// <param name="wpfShell">IShell optional</param>
        public WpfHostedService(ILogger<WpfHostedService> logger, IApplicationLifetime applicationLifetime, IWpfContext wpfContext, IWpfShell wpfShell = null)
        {
            _logger = logger;
            _applicationLifetime = applicationLifetime;
            _wpfContext = wpfContext;
            _shell = wpfShell as Window;
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Register to the host application lifetime ApplicationStopping to shutdown the WPF application
            _applicationLifetime.ApplicationStopping.Register(()  =>
            {
                if (_wpfContext.IsRunning)
                {
                    _logger.LogDebug("Stopping WPF application.");
                    _wpfContext.WpfApplication.Dispatcher.Invoke(() => _wpfContext.WpfApplication.Shutdown());
                }
            });

            // Register to the WPF application exit to stop the host application
            _wpfContext.WpfApplication.Exit += (s,e) =>
            {
                _wpfContext.IsRunning = false;
                if (_wpfContext.IsLifetimeLinked)
                {
                    _logger.LogDebug("Stopping host application due to WPF application exit.");
                    _applicationLifetime.StopApplication();
                }
            };


            // Run the application
            _wpfContext.WpfApplication.Dispatcher.Invoke(() =>
            {
                _wpfContext.IsRunning = true;
                if (_shell != null)
                {
                    _wpfContext.WpfApplication.Run(_shell);
                }
                else
                {
                    _wpfContext.WpfApplication.Run();
                }
            });
            
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _wpfContext.WpfApplication.Dispatcher.Invoke(() => _wpfContext.WpfApplication.Shutdown());
            return Task.CompletedTask;
        }
    }
}
