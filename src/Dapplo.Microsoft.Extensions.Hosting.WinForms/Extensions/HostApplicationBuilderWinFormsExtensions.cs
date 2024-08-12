// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Dapplo.Microsoft.Extensions.Hosting.WinForms.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.WinForms;

/// <summary>
/// This contains the WinForms extensions for Microsoft.Extensions.Hosting
/// </summary>
public static class HostApplicationBuilderWinFormsExtensions
{
    /// <summary>
    /// Defines that stopping the WinForms application also stops the host (application)
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <returns>IHostApplicationBuilder</returns>
    public static IHostApplicationBuilder UseWinFormsLifetime(this IHostApplicationBuilder hostApplicationBuilder) =>    
        InternalBuilderWinFormsUtility.UseWinFormsLifetime(hostApplicationBuilder);

    /// <summary>
    /// Configure an WinForms application
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <param name="configureAction">Action to configure the Application</param>
    /// <returns>IHostApplicationBuilder</returns>
    public static IHostApplicationBuilder ConfigureWinForms(this IHostApplicationBuilder hostApplicationBuilder, Action<IWinFormsContext> configureAction = null) =>    
        InternalBuilderWinFormsUtility.ConfigureWinForms(hostApplicationBuilder, configureAction);    

    /// <summary>
    /// Configure an WinForms application
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <param name="configureAction">Action to configure the Application</param>
    /// <typeparam name="TView">Type for the View</typeparam>
    /// <returns>IHostApplicationBuilder</returns>
    public static IHostApplicationBuilder ConfigureWinForms<TView>(this IHostApplicationBuilder hostApplicationBuilder, Action<IWinFormsContext> configureAction = null)
        where TView : Form =>
        InternalBuilderWinFormsUtility.ConfigureWinForms<TView>(hostApplicationBuilder, configureAction);    

    /// <summary>
    /// Specify a shell, the primary Form, to start
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <typeparam name="TShell">Type for the shell, must derive from Form and implement IWinFormsShell</typeparam>
    /// <returns>IHostApplicationBuilder</returns>
    public static IHostApplicationBuilder ConfigureWinFormsShell<TShell>(this IHostApplicationBuilder hostApplicationBuilder)
        where TShell : Form, IWinFormsShell =>    
        InternalBuilderWinFormsUtility.ConfigureWinFormsShell<TShell>(hostApplicationBuilder);    
}
