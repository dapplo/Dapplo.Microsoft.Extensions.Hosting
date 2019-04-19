// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.Plugins
{
    /// <summary>
    /// This interface is the connection between the host and the plug-in code
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Implementing this method allows a plug-in to configure the host.
        /// This makes it possible to add services etc
        /// </summary>
        /// <param name="hostBuilderContext">HostBuilderContext</param>
        /// <param name="serviceCollection">IServiceCollection</param>
        void ConfigureHost(HostBuilderContext hostBuilderContext, IServiceCollection serviceCollection);
    }
}
