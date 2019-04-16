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
        public Task StartAsync(CancellationToken cancellationToken)
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            // Create a thread which runs windows forms
            var newFormsThread = new Thread(FormsThreadStart)
            {
                IsBackground = true
            };
            // Set the apartment state
            newFormsThread.SetApartmentState(ApartmentState.STA);
            // Start the new Forms thread
            newFormsThread.Start(taskCompletionSource);
          
            return taskCompletionSource.Task;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (_winFormsContext.IsRunning)
            {
                _logger.LogDebug("Stopping WinForms application.");
                _winFormsContext.FormsDispatcher.Invoke(() =>
                {
                    Application.Exit();
                });
            }
            return Task.CompletedTask;
        }
        
        /// <summary>
        /// Start Windows Forms
        /// </summary>
        private void FormsThreadStart(object taskCompletionSourceAsObject)
        {
            TaskCompletionSource<bool> taskCompletionSource = taskCompletionSourceAsObject as TaskCompletionSource<bool>;

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
            taskCompletionSource.SetResult(true);

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
