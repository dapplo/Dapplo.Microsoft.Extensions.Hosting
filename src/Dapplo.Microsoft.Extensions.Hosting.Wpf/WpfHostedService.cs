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
        private readonly Window _shell;
        private readonly IApplicationLifetime _applicationLifetime;
        private Application _application;
        private readonly TaskScheduler _taskScheduler;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationLifetime"></param>
        /// <param name="shell"></param>
        public WpfHostedService(IApplicationLifetime applicationLifetime, IShell shell = null)
        {
            _shell = shell as Window;
            _applicationLifetime = applicationLifetime;
            
            _taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Register to the application lifetime ApplicationStopping to shutdown the application
            _applicationLifetime.ApplicationStopping.Register(()  =>
            {
                _application.Dispatcher.Invoke(() => _application.Shutdown());
            });

            // Create the application
            _application = new Application
            {
                ShutdownMode = ShutdownMode.OnLastWindowClose
            };
            
            // Register to the application exit to stop the application
            _application.Exit += (s,e) =>
            {
                _applicationLifetime.StopApplication();
            };

            // Run the application
            Task.Factory.StartNew(() =>
            {
                if (_shell != null)
                {
                    _application.Run(_shell);
                }
                else
                {
                    _application.Run();
                }
            }, default, TaskCreationOptions.DenyChildAttach, _taskScheduler);
            
            
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Task.Factory.StartNew(() =>
            {
                _application.Shutdown();
            }, default, TaskCreationOptions.DenyChildAttach, _taskScheduler);
            return Task.CompletedTask;
        }
    }
}
