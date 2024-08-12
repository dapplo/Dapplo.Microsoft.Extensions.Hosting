// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.Microsoft.Extensions.Hosting.WinUI.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;

namespace Dapplo.Microsoft.Extensions.Hosting.WinUI;

/// <summary>
/// This contains the WinUI extensions for Microsoft.Extensions.Hosting
/// </summary>
public static class HostBuilderWinUIExtensions
{
    /// <summary>
    /// Configure a WinUI application
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <returns></returns>
    public static IHostBuilder ConfigureWinUI<TApp, TAppWindow>(this IHostBuilder hostBuilder)
        where TApp : Application
        where TAppWindow : Window =>    
        InternalBuilderWinUIUtility.ConfigureWinUI<TApp, TAppWindow>(hostBuilder);    
}
