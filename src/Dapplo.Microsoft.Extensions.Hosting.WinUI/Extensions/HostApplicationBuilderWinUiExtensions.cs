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
public static class HostApplicationBuilderWinUiExtensions
{
    /// <summary>
    /// Configure a WinUI application
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <returns></returns>
    public static IHostApplicationBuilder ConfigureWinUI<TApp, TAppWindow>(this IHostApplicationBuilder hostApplicationBuilder)
        where TApp : Application
        where TAppWindow : Window =>    
        InternalBuilderWinUIUtility.ConfigureWinUI<TApp, TAppWindow>(hostApplicationBuilder);    
}
