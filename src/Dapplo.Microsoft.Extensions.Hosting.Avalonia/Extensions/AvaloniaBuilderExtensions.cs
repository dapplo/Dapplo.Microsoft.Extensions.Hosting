// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Avalonia;
using Avalonia.Controls;

namespace Dapplo.Microsoft.Extensions.Hosting.Avalonia;

/// <summary>
/// Extension methods to configure Avalonia
/// </summary>
public static class AvaloniaBuilderExtensions
{
    /// <summary>
    /// Register a window, as a singleton
    /// </summary>
    /// <typeparam name="TWindow">Type of the window, must inherit from Window</typeparam>
    /// <param name="avaloniaBuilder">IAvaloniaBuilder</param>
    /// <returns>IAvaloniaBuilder</returns>
    public static IAvaloniaBuilder UseWindow<TWindow>(this IAvaloniaBuilder avaloniaBuilder) where TWindow : Window
    {
        avaloniaBuilder.WindowTypes.Add(typeof(TWindow));
        return avaloniaBuilder;
    }

    /// <summary>
    /// Register a type for the main application
    /// </summary>
    /// <typeparam name="TApplication">Type of the application, must inherit from Application</typeparam>
    /// <param name="avaloniaBuilder">IAvaloniaBuilder</param>
    /// <returns>IAvaloniaBuilder</returns>
    public static IAvaloniaBuilder UseApplication<TApplication>(this IAvaloniaBuilder avaloniaBuilder) where TApplication : Application
    {
        avaloniaBuilder.ApplicationType = typeof(TApplication);
        return avaloniaBuilder;
    }

    /// <summary>
    /// Uses the current application.
    /// </summary>
    /// <typeparam name="TApplication">The type of the application.</typeparam>
    /// <param name="avaloniaBuilder">The Avalonia builder.</param>
    /// <param name="currentApplication">The current application.</param>
    /// <returns></returns>
    public static IAvaloniaBuilder UseCurrentApplication<TApplication>(this IAvaloniaBuilder avaloniaBuilder, TApplication currentApplication) where TApplication : Application
    {
        avaloniaBuilder.ApplicationType = typeof(TApplication);
        avaloniaBuilder.Application = currentApplication;
        return avaloniaBuilder;
    }

    /// <summary>
    /// Register action to configure the Application
    /// </summary>
    /// <param name="avaloniaBuilder">IAvaloniaBuilder</param>
    /// <param name="configureAction">Action to configure the Application</param>
    /// <returns>IAvaloniaBuilder</returns>
    public static IAvaloniaBuilder ConfigureContext(this IAvaloniaBuilder avaloniaBuilder, Action<IAvaloniaContext> configureAction)
    {
        avaloniaBuilder.ConfigureContextAction = configureAction;
        return avaloniaBuilder;
    }

    /// <summary>
    /// Register action to configure the AppBuilder
    /// </summary>
    /// <param name="avaloniaBuilder">IAvaloniaBuilder</param>
    /// <param name="configureAction">Action to configure the AppBuilder</param>
    /// <returns>IAvaloniaBuilder</returns>
    public static IAvaloniaBuilder ConfigureAppBuilder(this IAvaloniaBuilder avaloniaBuilder, Action<AppBuilder> configureAction)
    {
        avaloniaBuilder.ConfigureAppBuilderAction = configureAction;
        return avaloniaBuilder;
    }
}
