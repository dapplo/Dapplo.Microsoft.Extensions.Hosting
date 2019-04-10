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
                TryRetrieveWpfContext(hostBuilder.Properties, out var wpfContext);
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
        /// <typeparam name="TShell">Type for the shell, must derive from Window and implement IShell</typeparam>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder ConfigureWpf<TShell>(this IHostBuilder hostBuilder, Action<IWpfContext> configureAction = null) where TShell : Window, IWpfShell
        {
            hostBuilder.ConfigureWpf(configureAction);
            hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                serviceCollection.AddSingleton<IWpfShell, TShell>();
            });
            return hostBuilder;
        }

        
        /// <summary>
        /// Specify the shell, the primary window, to start
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <typeparam name="TShell">Type for the shell, must derive from Window and implement IShell</typeparam>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder ConfigureWpfShell<TShell>(this IHostBuilder hostBuilder) where TShell : Window, IWpfShell
        {
            return hostBuilder.ConfigureWpf<TShell>();
        }
    }
}