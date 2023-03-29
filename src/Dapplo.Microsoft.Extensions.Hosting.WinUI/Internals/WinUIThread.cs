// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.Microsoft.Extensions.Hosting.UiThread;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using System;
using System.Threading;
using WinRT;

namespace Dapplo.Microsoft.Extensions.Hosting.WinUI.Internals
{
    /// <summary>
    /// This contains the logic for the WinUI thread
    /// </summary>
    public class WinUIThread : BaseUiThread<IWinUIContext>
    {
        /// <summary>
        /// This will create the WinUIThread
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider</param>
        public WinUIThread(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <inheritdoc />
        protected override void PreUiThreadStart() =>
            ComWrappersSupport.InitializeComWrappers();

        /// <inheritdoc />
        protected override void UiThreadStart()
        {
            Application.Start(_ =>
            {
                UiContext.Dispatcher = DispatcherQueue.GetForCurrentThread();
                DispatcherQueueSynchronizationContext context = new(UiContext.Dispatcher);
                SynchronizationContext.SetSynchronizationContext(context);

                UiContext.WinUIApplication = ServiceProvider.GetRequiredService<Application>();
                // Use the provided IWinUIService
                var winUIServices = ServiceProvider.GetServices<IWinUIService>();
                if (winUIServices != null)
                {
                    foreach (var winUIService in winUIServices)
                    {
                        winUIService.Initialize(UiContext.WinUIApplication);
                    }
                }

                UiContext.AppWindow = (Window)ActivatorUtilities.CreateInstance(ServiceProvider, UiContext.AppWindowType);
                UiContext.AppWindow.Activate();
            });
            HandleApplicationExit();
        }
    }
}
