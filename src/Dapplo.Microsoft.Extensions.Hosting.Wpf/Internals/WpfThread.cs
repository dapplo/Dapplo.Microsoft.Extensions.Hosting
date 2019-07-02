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
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace Dapplo.Microsoft.Extensions.Hosting.Wpf.Internals
{
    /// <summary>
    /// This contains the logic for the WPF thread
    /// </summary>
    internal class WpfThread
    {
        private readonly ManualResetEvent _serviceManualResetEvent = new ManualResetEvent(false);
        private readonly IWpfContext _wpfContext;
        private IServiceProvider _serviceProvider;
            
        /// <summary>
        /// Constructor which is called from the IWpfContext
        /// </summary>
        /// <param name="wpfContext">IWpfContext</param>
        public WpfThread(IWpfContext wpfContext)
        {
            _wpfContext = wpfContext;
            // Create a thread which runs WPF
            var newWpfThread = new Thread(WpfThreadStart)
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
        /// Start WPF on the previously created UI Thread
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
                    //_logger.LogDebug("Stopping host application due to WPF application exit.");
                    _serviceProvider.GetService<IHostApplicationLifetime>().StopApplication();
                }
            };
            
            // Store the application for others to interact
            _wpfContext.WpfApplication = wpfApplication;

            // Wait for the startup
            _serviceManualResetEvent.WaitOne();

            // Mark the application as running
            _wpfContext.IsRunning = true;

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