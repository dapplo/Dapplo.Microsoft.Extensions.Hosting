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

using System.Collections.Generic;
using Caliburn.Micro;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.CaliburnMicro
{
    /// <summary>
    /// This contains the CaliburnMicro extensions for Microsoft.Extensions.Hosting 
    /// </summary>
    public static class HostBuilderCaliburnMicroExtensions
    {
        private const string CaliburnMicroContextKey = "CaliburnMicroContext";
        
        /// <summary>
        /// Helper method to retrieve the 
        /// </summary>
        /// <param name="properties">IDictionary</param>
        /// <returns>bool if there was already an </returns>
        private static bool TryRetrieveWpfContext(this IDictionary<object, object> properties)
        {
            if (properties.TryGetValue(CaliburnMicroContextKey, out var wpfContextAsObject))
            {
                return true;

            }
            properties[CaliburnMicroContextKey] = null;
            return false;
        }
        
        /// <summary>
        /// Configure Caliburn.Micro
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder ConfigureCaliburnMicro(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                if (!TryRetrieveWpfContext(hostBuilder.Properties))
                {
                    serviceCollection.AddSingleton<IWindowManager, DapploWindowManager>();
                    serviceCollection.AddHostedService<CaliburnMicroBootstrapper>();
                }
            });
            return hostBuilder;
        }

        /// <summary>
        /// Configure Caliburn.Micro with the shell
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder ConfigureCaliburnMicro<TShell>(this IHostBuilder hostBuilder) where TShell : class, ICaliburnMicroShell
        {
            hostBuilder.ConfigureCaliburnMicro();
            hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                serviceCollection.AddSingleton<ICaliburnMicroShell, TShell>();
            });

            return hostBuilder;
        }

    }
}