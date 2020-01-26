// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.UiThread
{
    /// <summary>
    /// This contains the logic for the WinForms thread
    /// </summary>
    public abstract class BaseUiThread<T> where T : IUiContext
    {
        private readonly ManualResetEvent _serviceManualResetEvent = new ManualResetEvent(false);
        /// <summary>
        /// The IUiContext
        /// </summary>
        protected readonly T _uiContext;

        /// <summary>
        /// The IServiceProvider used by all IUiContect implementations
        /// </summary>
        protected readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor which is called from the IWinFormsContext
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider</param>
        public BaseUiThread(IServiceProvider serviceProvider)
        {
            _uiContext = serviceProvider.GetService<T>(); ;
            _serviceProvider = serviceProvider;
            // Create a thread which runs the UI
            var newUiThread = new Thread(InternalUiThreadStart)
            {
                IsBackground = true
            };
            // Set the apartment state
            newUiThread.SetApartmentState(ApartmentState.STA);
            // Start the new UI thread
            newUiThread.Start();
        }

        /// <summary>
        /// Start the DI service on the thread
        /// </summary>
        public void Start()
        {
            // Make the UI thread go
            _serviceManualResetEvent.Set();
        }

        /// <summary>
        /// Start UI
        /// </summary>
        private void InternalUiThreadStart()
        {
            // Do the pre initialization, if any
            PreUiThreadStart();
            // Wait for the startup
            _serviceManualResetEvent.WaitOne();
            // Run the application
            _uiContext.IsRunning = true;
            // Run the actuall code
            UiThreadStart();
        }

        /// <summary>
        /// Do all the pre work, before the UI thread can start
        /// </summary>
        protected abstract void PreUiThreadStart();

        /// <summary>
        /// Implement all the code which is needed to run the actual UI
        /// </summary>
        protected abstract void UiThreadStart();

        /// <summary>
        /// Handle the application exit
        /// </summary>
        protected void HandleApplicationExit()
        {
            _uiContext.IsRunning = false;
            if (!_uiContext.IsLifetimeLinked)
            {
                return;
            }

            //_logger.LogDebug("Stopping host application due to WinForms application exit.");
            _serviceProvider.GetService<IHostApplicationLifetime>().StopApplication();
        }
    }
}