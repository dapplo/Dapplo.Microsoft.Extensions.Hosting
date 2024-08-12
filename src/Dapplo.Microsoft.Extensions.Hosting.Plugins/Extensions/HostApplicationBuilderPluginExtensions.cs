// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Dapplo.Microsoft.Extensions.Hosting.Plugins.Extensions;

#if NETCOREAPP
using System.Runtime.Loader;
#endif
using Dapplo.Microsoft.Extensions.Hosting.Plugins.Internals;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.Plugins;

/// <summary>
/// Extensions for adding plug-ins to your host
/// </summary>
public static class HostApplicationBuilderPluginExtensions
{
    /// <summary>
    /// Configure the plugins
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <param name="configurePlugin">Action to configure the IPluginBuilder</param>
    /// <returns>IHostApplicationBuilder</returns>
    public static IHostApplicationBuilder ConfigurePlugins(this IHostApplicationBuilder hostApplicationBuilder, Action<IPluginBuilder> configurePlugin) =>    
        InternalBuilderPluginUtility.ConfigurePlugins(hostApplicationBuilder, configurePlugin);
}
