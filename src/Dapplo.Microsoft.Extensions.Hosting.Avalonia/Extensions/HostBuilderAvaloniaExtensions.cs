// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Avalonia.Controls;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.Avalonia;

/// <summary>
/// This contains the Avalonia extensions for Microsoft.Extensions.Hosting
/// </summary>
public static class HostBuilderAvaloniaExtensions
{
    /// <summary>
    /// Defines that stopping the Avalonia application also stops the host (application)
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <param name="shutdownMode">ShutdownMode default is OnLastWindowClose</param>
    /// <returns>IHostBuilder</returns>
    public static IHostBuilder UseAvaloniaLifetime(this IHostBuilder hostBuilder, ShutdownMode shutdownMode = ShutdownMode.OnLastWindowClose) =>    
        InternalBuilderAvaloniaUtility.UseAvaloniaLifetime(hostBuilder, shutdownMode);

    /// <summary>
    /// Configure an Avalonia application 
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <param name="configureDelegate">Action to configure Avalonia</param>
    /// <returns></returns>
    public static IHostBuilder ConfigureAvalonia(this IHostBuilder hostBuilder, Action<IAvaloniaBuilder> configureDelegate = null) => 
        InternalBuilderAvaloniaUtility.ConfigureAvalonia(hostBuilder, configureDelegate);
}
