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
using Dapplo.Microsoft.Extensions.Hosting.AppServices.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.AppServices
{
    /// <summary>
    /// Extensions for loading plugins
    /// </summary>
    public static class HostBuilderApplicationExtensions
    {
        private const string MutexBuilderKey = "MutexBuilder";

        /// <summary>
        /// Helper method to retrieve the mutex builder
        /// </summary>
        /// <param name="properties">IDictionary</param>
        /// <param name="mutexBuilder">IMutexBuilder out value</param>
        /// <returns>bool if there was a matcher</returns>
        private static bool TryRetrieveMutexBuilder(this IDictionary<object, object> properties, out IMutexBuilder mutexBuilder)
        {
            if (properties.TryGetValue(MutexBuilderKey, out var mutexBuilderObject))
            {
                mutexBuilder = mutexBuilderObject as IMutexBuilder;
                return true;

            }
            mutexBuilder = new MutexBuilder();
            properties[MutexBuilderKey] = mutexBuilder;
            return false;
        }
        
        /// <summary>
        /// Prevent that an application runs multiple times
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="configureAction">Action to configure IMutexBuilder</param>
        /// <returns>IHostBuilder for fluently calling</returns>
        public static IHostBuilder ConfigureSingleInstance(this IHostBuilder hostBuilder, Action<IMutexBuilder> configureAction) 
        {
            hostBuilder.ConfigureServices((hostContext, serviceCollection) =>
            {
                if (!TryRetrieveMutexBuilder(hostBuilder.Properties, out var mutexBuilder))
                {
                    serviceCollection.AddSingleton(mutexBuilder);
                    serviceCollection.AddHostedService<MutexLifetimeService>();
                }
                configureAction?.Invoke(mutexBuilder);
            });
            return hostBuilder;
        }

        /// <summary>
        /// Prevent that an application runs multiple times
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="mutexId">string</param>
        /// <returns>IHostBuilder for fluently calling</returns>
        public static IHostBuilder ConfigureSingleInstance(this IHostBuilder hostBuilder, string mutexId) 
        {
            return hostBuilder.ConfigureSingleInstance(builder => builder.MutexId = mutexId);
        }
        
    }
}
