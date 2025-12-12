// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Avalonia.Controls;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.Avalonia;

/// <summary>
/// This contains the Avalonia extensions for Microsoft.Extensions.Hosting with HostApplicationBuilder
/// </summary>
public static class HostApplicationBuilderAvaloniaExtensions
{
    /// <summary>
    /// Defines that stopping the Avalonia application also stops the host (application)
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <param name="shutdownMode">ShutdownMode default is OnLastWindowClose</param>
    /// <returns>IHostApplicationBuilder</returns>
    public static IHostApplicationBuilder UseAvaloniaLifetime(this IHostApplicationBuilder hostApplicationBuilder, ShutdownMode shutdownMode = ShutdownMode.OnLastWindowClose) =>
        InternalBuilderAvaloniaUtility.UseAvaloniaLifetime(hostApplicationBuilder, shutdownMode);

    /// <summary>
    /// Configure an Avalonia application 
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <param name="configureDelegate">Action to configure Avalonia</param>
    /// <returns></returns>
    public static IHostApplicationBuilder ConfigureAvalonia(this IHostApplicationBuilder hostApplicationBuilder, Action<IAvaloniaBuilder> configureDelegate = null) =>
        InternalBuilderAvaloniaUtility.ConfigureAvalonia(hostApplicationBuilder, configureDelegate);
}
