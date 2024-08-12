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
public static class HostBuilderWinFormsExtensions
{
    /// <summary>
    /// Defines that stopping the WinForms application also stops the host (application)
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <returns>IHostBuilder</returns>
    public static IHostBuilder UseWinFormsLifetime(this IHostBuilder hostBuilder) =>    
        InternalBuilderWinFormsUtility.UseWinFormsLifetime(hostBuilder);    

    /// <summary>
    /// Configure an WinForms application
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <param name="configureAction">Action to configure the Application</param>
    /// <returns>IHostBuilder</returns>
    public static IHostBuilder ConfigureWinForms(this IHostBuilder hostBuilder, Action<IWinFormsContext> configureAction = null) =>    
        InternalBuilderWinFormsUtility.ConfigureWinForms(hostBuilder, configureAction);    

    /// <summary>
    /// Configure an WinForms application
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <param name="configureAction">Action to configure the Application</param>
    /// <typeparam name="TView">Type for the View</typeparam>
    /// <returns>IHostBuilder</returns>
    public static IHostBuilder ConfigureWinForms<TView>(this IHostBuilder hostBuilder, Action<IWinFormsContext> configureAction = null)
        where TView : Form =>
        InternalBuilderWinFormsUtility.ConfigureWinForms<TView>(hostBuilder, configureAction);

    /// <summary>
    /// Specify a shell, the primary Form, to start
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <typeparam name="TShell">Type for the shell, must derive from Form and implement IWinFormsShell</typeparam>
    /// <returns>IHostBuilder</returns>
    public static IHostBuilder ConfigureWinFormsShell<TShell>(this IHostBuilder hostBuilder)
        where TShell : Form, IWinFormsShell =>
        InternalBuilderWinFormsUtility.ConfigureWinFormsShell<TShell>(hostBuilder);        
}
