// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Windows;
using Dapplo.Microsoft.Extensions.Hosting.Wpf.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.Wpf
{
    /// <summary>
    /// This contains the WPF extensions for Microsoft.Extensions.Hosting
    /// </summary>
    public static class HostBuilderWpfExtensions
    {
        private const string WpfContextKey = "WpfContext";

        /// <summary>
        /// Helper method to retrieve the IWpfContext
        /// </summary>
        /// <param name="properties">IDictionary</param>
        /// <param name="wpfContext">IWpfContext out value</param>
        /// <returns>bool if there was already an IWpfContext</returns>
        private static bool TryRetrieveWpfContext(this IDictionary<object, object> properties, out IWpfContext wpfContext)
        {
            if (properties.TryGetValue(WpfContextKey, out var wpfContextAsObject))
            {
                wpfContext = wpfContextAsObject as IWpfContext;
                return true;

            }
            wpfContext = new WpfContext();
            properties[WpfContextKey] = wpfContext;
            return false;
        }

        /// <summary>
        /// Defines that stopping the WPF application also stops the host (application)
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="shutdownMode">ShutdownMode default is OnLastWindowClose</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder UseWpfLifetime(this IHostBuilder hostBuilder, ShutdownMode shutdownMode = ShutdownMode.OnLastWindowClose)
        {
            hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                if (!TryRetrieveWpfContext(hostBuilder.Properties, out var wpfContext))
                {
                    throw new NotSupportedException("Please configure WPF first!");
                }

                wpfContext.ShutdownMode = shutdownMode;
                wpfContext.IsLifetimeLinked = true;
            });
            return hostBuilder;
        }

        /// <summary>
        /// Configure an WPF application
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="configureAction">Action to configure the Application</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder ConfigureWpf(this IHostBuilder hostBuilder, Action<IWpfContext> configureAction = null)
        {
            hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                if (!TryRetrieveWpfContext(hostBuilder.Properties, out var wpfContext))
                {
                    serviceCollection.AddSingleton(wpfContext);
                    serviceCollection.AddSingleton(serviceProvider => new WpfThread(serviceProvider));
                    serviceCollection.AddHostedService<WpfHostedService>();
                }
                configureAction?.Invoke(wpfContext);
            });
            return hostBuilder;
        }

        /// <summary>
        /// Configure an WPF application
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="configureAction">Action to configure the Application</param>
        /// <typeparam name="TView">
        ///     Type for the view, must derive from Window.
            /// If derived from IWpfShell, it automatically shown at start</typeparam>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder ConfigureWpf<TView>(this IHostBuilder hostBuilder, Action<IWpfContext> configureAction = null) where TView : Window
        {
            hostBuilder.ConfigureWpf(configureAction);
            hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                serviceCollection.AddSingleton<TView>();

                // Check if it also implements IWpfShell so we can register it as this
                var viewType = typeof(TView);
                var shellInterfaceType = typeof(IWpfShell);
                if (shellInterfaceType.IsAssignableFrom(viewType))
                {
                    serviceCollection.AddSingleton(shellInterfaceType, serviceProvider => serviceProvider.GetRequiredService<TView>());
                }
            });
            return hostBuilder;
        }
    }
}
