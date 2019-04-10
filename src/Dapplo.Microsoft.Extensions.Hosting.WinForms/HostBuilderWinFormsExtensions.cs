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
using System.Windows.Forms;
using Dapplo.Microsoft.Extensions.Hosting.WinForms.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.WinForms
{
    /// <summary>
    /// This contains the WinForms extensions for Microsoft.Extensions.Hosting 
    /// </summary>
    public static class HostBuilderWinFormsExtensions
    {
        private const string WinFormsContextKey = "WinFormsContext";
        
        /// <summary>
        /// Helper method to retrieve the IWinFormsContext
        /// </summary>
        /// <param name="properties">IDictionary</param>
        /// <param name="winFormsContext">IWinFormsContext out value</param>
        /// <returns>bool if there was already an IWinFormsContext</returns>
        private static bool TryRetrieveWinFormsContext(this IDictionary<object, object> properties, out IWinFormsContext winFormsContext)
        {
            if (properties.TryGetValue(WinFormsContextKey, out var winFormsContextAsObject))
            {
                winFormsContext = winFormsContextAsObject as IWinFormsContext;
                return true;

            }
            winFormsContext = new WinFormsContext();
            properties[WinFormsContextKey] = winFormsContext;
            return false;
        }

        /// <summary>
        /// Defines that stopping the WinForms application also stops the host (application) 
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="shutdownMode">ShutdownMode default is OnLastWindowClose</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder UseWinFormsLifetime(this IHostBuilder hostBuilder, ShutdownMode shutdownMode = ShutdownMode.OnLastWindowClose)
        {
            hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                TryRetrieveWinFormsContext(hostBuilder.Properties, out var winFormsContext);
                winFormsContext.IsLifetimeLinked = true;
            });
            return hostBuilder;
        }
        
        /// <summary>
        /// Configure an WinForms application
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="configureAction">Action to configure the Application</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder ConfigureWinForms(this IHostBuilder hostBuilder, Action<IWinFormsContext> configureAction = null)
        {
            hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                if (!TryRetrieveWinFormsContext(hostBuilder.Properties, out var winFormsContext))
                {
                    serviceCollection.AddSingleton(winFormsContext);
                    serviceCollection.AddHostedService<WinFormsHostedService>();
                }
                configureAction?.Invoke(winFormsContext);
            });
            return hostBuilder;
        }
        
        /// <summary>
        /// Configure an WinForms application
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="configureAction">Action to configure the Application</param>
        /// <typeparam name="TShell">Type for the shell, must derive from Form and implement IWinFormsShell</typeparam>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder ConfigureWinForms<TShell>(this IHostBuilder hostBuilder, Action<IWinFormsContext> configureAction = null) where TShell : Form, IWinFormsShell
        {
            hostBuilder.ConfigureWinForms(configureAction);
            hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                serviceCollection.AddSingleton<IWinFormsShell, TShell>();
            });
            return hostBuilder;
        }

        
        /// <summary>
        /// Specify the shell, the primary Form, to start
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <typeparam name="TShell">Type for the shell, must derive from Form and implement IWinFormsShell</typeparam>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder ConfigureWinFormsShell<TShell>(this IHostBuilder hostBuilder) where TShell : Form, IWinFormsShell
        {
            return hostBuilder.ConfigureWinForms<TShell>();
        }
    }
}