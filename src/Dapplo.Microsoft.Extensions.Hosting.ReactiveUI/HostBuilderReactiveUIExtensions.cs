// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.ReactiveUI
{
    /// <summary>
    /// This contains the rReactiveUi extensions for Microsoft.Extensions.Hosting 
    /// </summary>
    public static class HostBuilderReactiveUiExtensions
    {
        /// <summary>
        /// Configure a ReactiveUI application
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder ConfigureReactiveUi(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                serviceCollection.AddSingleton<GenericHostDependencyResolver>();
            });
            return hostBuilder;
        }
   }
}