// Dapplo - building blocks for desktop applications
// Copyright (C) 2019 Dapplo
// 
// For more information see: http://dapplo.net/
// Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
// This file is part of Dapplo.Hosting.Samples
// 
// Dapplo.Hosting.Samples is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Dapplo.Hosting.Samples is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have a copy of the GNU Lesser General Public License
// along with Dapplo.Hosting.Samples. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

using System;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.WinForms.Internals
{
    /// <summary>
    /// This contains the logic for the WinForms thread
    /// </summary>
    internal class WinFormsThread
    {
        private readonly ManualResetEvent _serviceManualResetEvent = new ManualResetEvent(false);
        private readonly IWinFormsContext _winFormsContext;
        private IServiceProvider _serviceProvider;
            
        /// <summary>
        /// Constructor which is called from the IWinFormsContext
        /// </summary>
        /// <param name="winFormsContext">IWinFormsContext</param>
        public WinFormsThread(IWinFormsContext winFormsContext)
        {
            _winFormsContext = winFormsContext;
            // Create a thread which runs WPF
            var newWpfThread = new Thread(WinFormsThreadStart)
            {
                IsBackground = true
            };
            // Set the apartment state
            newWpfThread.SetApartmentState(ApartmentState.STA);
            // Start the new WPF thread
            newWpfThread.Start();
        }

        /// <summary>
        /// Start the DI service on the thread
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider</param>
        public void Start(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            // Make the UI thread go
            _serviceManualResetEvent.Set();
        }
        
        /// <summary>
        /// Start Windows Forms
        /// </summary>
        private void WinFormsThreadStart()
        {
            var currentDispatcher = Dispatcher.CurrentDispatcher;
            _winFormsContext.Dispatcher = currentDispatcher;

            // Create our SynchronizationContext, and install it:
            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(currentDispatcher));

            if (_winFormsContext.EnableVisualStyles)
            {
                Application.EnableVisualStyles();
            }

            // Register to the WinForms application exit to stop the host application
            Application.ApplicationExit += OnApplicationExit;

            // Wait for the startup
            _serviceManualResetEvent.WaitOne();
            
            // Run the application
            _winFormsContext.IsRunning = true;
            
            // Use the provided IWinFormsService
            var winFormServices = _serviceProvider.GetServices<IWinFormsService>();
            if (winFormServices != null)
            {
                foreach(var winFormService in winFormServices)
                {
                    winFormService.Initialize();
                }
            }
            
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
            if (!_winFormsContext.IsLifetimeLinked)
            {
                return;
            }

            //_logger.LogDebug("Stopping host application due to WinForms application exit.");
            _serviceProvider.GetService<IHostApplicationLifetime>().StopApplication();
        }
    }
}