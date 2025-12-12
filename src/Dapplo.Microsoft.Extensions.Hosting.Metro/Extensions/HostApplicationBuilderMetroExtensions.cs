// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Dapplo.Microsoft.Extensions.Hosting.Metro.Internals;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.Metro;

/// <summary>
/// This contains the Metro extensions for Microsoft.Extensions.Hosting 
/// </summary>
public static class HostApplicationBuilderMetroExtensions
{    
    /// <summary>
    /// This enables MahApps.Metro
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <param name="configureAction">Action to configure IMetroContext</param>
    /// <returns>IHostApplicationBuilder</returns>
    public static IHostApplicationBuilder ConfigureMetro(this IHostApplicationBuilder hostApplicationBuilder, Action<IMetroContext> configureAction = null) =>
        InternalBuilderMetroUtility.ConfigureMetro(hostApplicationBuilder, configureAction);
    

    /// <summary>
    /// Configure WPF to use MahApps.Metro and specify the theme
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <param name="theme">string</param>
    /// <returns>IHostApplicationBuilder</returns>
    public static IHostApplicationBuilder ConfigureMetro(this IHostApplicationBuilder hostApplicationBuilder, string theme) =>    
        InternalBuilderMetroUtility.ConfigureMetro(hostApplicationBuilder, theme);
}
