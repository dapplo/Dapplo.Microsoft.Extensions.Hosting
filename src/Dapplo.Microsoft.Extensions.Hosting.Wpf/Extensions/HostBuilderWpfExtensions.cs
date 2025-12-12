// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.Wpf;

/// <summary>
/// This contains the WPF extensions for Microsoft.Extensions.Hosting
/// </summary>
public static class HostBuilderWpfExtensions
{
    /// <summary>
    /// Defines that stopping the WPF application also stops the host (application)
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <param name="shutdownMode">ShutdownMode default is OnLastWindowClose</param>
    /// <returns>IHostBuilder</returns>
    public static IHostBuilder UseWpfLifetime(this IHostBuilder hostBuilder, ShutdownMode shutdownMode = ShutdownMode.OnLastWindowClose) =>    
        InternalBuilderWpfUtility.UseWpfLifetime(hostBuilder, shutdownMode);

    /// <summary>
    /// Configure an WPF application 
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <param name="configureDelegate">Action to configure Wpf</param>
    /// <returns></returns>
    public static IHostBuilder ConfigureWpf(this IHostBuilder hostBuilder, Action<IWpfBuilder> configureDelegate = null) => 
        InternalBuilderWpfUtility.ConfigureWpf(hostBuilder, configureDelegate);
}
