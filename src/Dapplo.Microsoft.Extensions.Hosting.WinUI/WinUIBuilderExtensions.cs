// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.UI.Xaml;
using System;

namespace Dapplo.Microsoft.Extensions.Hosting.WinUI
{
    /// <summary>
    /// Extension methods to configure WinUI
    /// </summary>
    public static class WinUIBuilderExtensions
    {
        /// <summary>
        /// Register action to configure the Application
        /// </summary>
        /// <param name="winUIBuilder">IWinUIBuilder</param>
        /// <param name="configureAction">Action to configure the Application</param>
        /// <returns>IWinUIBuilder</returns>
        public static IWinUIBuilder ConfigureContext(this IWinUIBuilder winUIBuilder, Action<IWinUIContext> configureAction)
        {
            winUIBuilder.ConfigureContextAction = configureAction;
            return winUIBuilder;
        }

        /// <summary>
        /// Register a window, as a singleton
        /// </summary>
        /// <typeparam name="TWindow">Type of the window, must inherit from Window</typeparam>
        /// <param name="winUIBuilder">IWinUIBuilder</param>
        /// <returns>IWinUIBuilder</returns>
        public static IWinUIBuilder UseWindow<TWindow>(this IWinUIBuilder winUIBuilder) where TWindow : Window
        {
            winUIBuilder.WindowTypes.Add(typeof(TWindow));
            return winUIBuilder;
        }
    }
}
