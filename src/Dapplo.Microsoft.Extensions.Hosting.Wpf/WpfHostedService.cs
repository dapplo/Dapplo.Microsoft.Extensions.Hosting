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

namespace Dapplo.Microsoft.Extensions.Hosting.Wpf
{
    /// <summary>
    /// This hosts a WPF service, making sure the lifecycle is managed
    /// </summary>
    public class WpfHostedService : IHostedService
    {
        private readonly IApplicationLifetime _applicationLifetime;
        private readonly IWpfBuilder _wpfBuilder;
        private readonly Window _shell;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationLifetime"></param>
        /// <param name="wpfBuilder">IWpfBuilder</param>
        /// <param name="shell">IShell optional</param>
        public WpfHostedService(IApplicationLifetime applicationLifetime, IWpfBuilder wpfBuilder, IShell shell = null)
        {
            _applicationLifetime = applicationLifetime;
            _wpfBuilder = wpfBuilder;
            _shell = shell as Window;
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Register to the application lifetime ApplicationStopping to shutdown the application
            _applicationLifetime.ApplicationStopping.Register(()  =>
            {
                _wpfBuilder.WpfApplication.Dispatcher.Invoke(() => _wpfBuilder.WpfApplication.Shutdown());
            });
            
            // Register to the application exit to stop the application
            _wpfBuilder.WpfApplication.Exit += (s,e) =>
            {
                _applicationLifetime.StopApplication();
            };

            // Run the application
            _wpfBuilder.WpfApplication.Dispatcher.Invoke(() =>
            {
                if (_shell != null)
                {
                    _wpfBuilder.WpfApplication.Run(_shell);
                }
                else
                {
                    _wpfBuilder.WpfApplication.Run();
                }
            });
            
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _wpfBuilder.WpfApplication.Dispatcher.Invoke(() => _wpfBuilder.WpfApplication.Shutdown());
            return Task.CompletedTask;
        }
    }
}
