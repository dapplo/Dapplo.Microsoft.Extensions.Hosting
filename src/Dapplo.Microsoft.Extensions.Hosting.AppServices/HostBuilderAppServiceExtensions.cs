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
        /// <summary>
        /// Prevent that an application runs multiple times
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="mutexId">Mutex ID</param>
        /// <param name="whenNotFirstInstance">Action which is called when the mutex can't be locked</param>
        /// <param name="global">bool specifying if the mutex is global, one instance on a Windows instance, or local one instance per session (is default)</param>
        /// <returns>IHostBuilder for fluently calling</returns>
        public static IHostBuilder ForceSingleInstance(this IHostBuilder hostBuilder, string mutexId, Action<IHostingEnvironment> whenNotFirstInstance = null, bool global = false)
        {
            hostBuilder.ConfigureServices((hostContext, serviceCollection) =>
            {
                serviceCollection.AddSingleton(new MutexConfig
                {
                    MutexId = mutexId,
                    WhenNotFirstInstance = whenNotFirstInstance,
                    IsGlobal = global
                });
                serviceCollection.AddHostedService<MutexLifetimeService>();
            });
            return hostBuilder;
        }
    }
}
