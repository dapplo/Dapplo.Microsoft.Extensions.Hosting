// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.Microsoft.Extensions.Hosting.WinUI.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;

namespace Dapplo.Microsoft.Extensions.Hosting.WinUI
{
    /// <summary>
    /// This contains the WinUI extensions for Microsoft.Extensions.Hosting
    /// </summary>
    public static class HostBuilderWinUIExtensions
    {
        private const string WinUIContextKey = "WinUIContext";

        /// <summary>
        /// Configure a WinUI application
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="configureDelegate">Action to configure WinUI</param>
        /// <returns></returns>
        public static IHostBuilder ConfigureWinUI<TApp>(this IHostBuilder hostBuilder, Action<IWinUIBuilder> configureDelegate = null) where TApp : Application
        {
            var appType = typeof(TApp);
            WinUIBuilder builder = new();
            configureDelegate?.Invoke(builder);

            hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                if (!TryRetrieveWinUIContext(hostBuilder.Properties, out var winUIContext))
                {
                    serviceCollection.AddSingleton(winUIContext);
                    serviceCollection.AddSingleton(serviceProvider => new WinUIThread(serviceProvider));
                    serviceCollection.AddHostedService<WinUIHostedService>();
                }

                foreach (var item in builder.WindowTypes)
                {
                    winUIContext.WindowTypes.Add(item);
                }
                winUIContext.IsLifetimeLinked = true;
            });

            if (appType != null)
            {
                var baseApplicationType = typeof(Application);
                if (!baseApplicationType.IsAssignableFrom(appType))
                {
                    throw new ArgumentException("The registered Application type inherit System.Windows.Application", nameof(TApp));
                }

                hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
                {
                    serviceCollection.AddSingleton<TApp>();

                    if (appType != baseApplicationType)
                    {
                        serviceCollection.AddSingleton<Application>(services => services.GetRequiredService<TApp>());
                    }
                });
            }

            return hostBuilder;
        }

        /// <summary>
        /// Helper method to retrieve the IWinUIContext
        /// </summary>
        /// <param name="properties">IDictionary</param>
        /// <param name="winUIContext">IWinUIContext out value</param>
        /// <returns>bool if there was already an IWinUIContext</returns>
        private static bool TryRetrieveWinUIContext(this IDictionary<object, object> properties, out IWinUIContext winUIContext)
        {
            if (properties.TryGetValue(WinUIContextKey, out var winUIContextAsObject))
            {
                winUIContext = (IWinUIContext)winUIContextAsObject;
                return true;
            }
            winUIContext = new WinUIContext();
            properties[WinUIContextKey] = winUIContext;
            return false;
        }
    }
}
