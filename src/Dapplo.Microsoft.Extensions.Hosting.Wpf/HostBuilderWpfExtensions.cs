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
        private const string WpfBuilderKey = "WpfBuilder";
        
        /// <summary>
        /// Helper method to retrieve the mutex builder
        /// </summary>
        /// <param name="properties">IDictionary</param>
        /// <param name="wpfBuilder">IMutexBuilder out value</param>
        /// <returns>bool if there was a matcher</returns>
        private static bool TryRetrieveWpfBuilder(this IDictionary<object, object> properties, out IWpfBuilder wpfBuilder)
        {
            if (properties.TryGetValue(WpfBuilderKey, out var wpfBuilderObject))
            {
                wpfBuilder = wpfBuilderObject as IWpfBuilder;
                return true;

            }
            wpfBuilder = new WpfBuilder();
            properties[WpfBuilderKey] = wpfBuilder;
            return false;
        }
        
        /// <summary>
        /// Configure an WPF application
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="configureAction">Action to configure the Application</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder ConfigureWpf(this IHostBuilder hostBuilder, Action<IWpfBuilder> configureAction = null)
        {
            hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                if (!TryRetrieveWpfBuilder(hostBuilder.Properties, out var wpfBuilder))
                {
                    serviceCollection.AddSingleton(wpfBuilder);
                    serviceCollection.AddHostedService<WpfHostedService>();
                }
                configureAction?.Invoke(wpfBuilder);
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
        public static IHostBuilder ConfigureWpf<TShell>(this IHostBuilder hostBuilder, Action<IWpfBuilder> configureAction = null) where TShell : Window, IShell
        {
            hostBuilder.ConfigureWpf(configureAction);
            hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                serviceCollection.AddSingleton<IShell, TShell>();
            });
            return hostBuilder;
        }

        
        /// <summary>
        /// Specify the shell, the primary window, to start
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <typeparam name="TShell">Type for the shell, must derive from Window and implement IShell</typeparam>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder ConfigureWpfShell<TShell>(this IHostBuilder hostBuilder) where TShell : Window, IShell
        {
            return hostBuilder.ConfigureWpf<TShell>();
        }
    }
}