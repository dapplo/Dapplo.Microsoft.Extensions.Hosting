// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;

namespace Dapplo.Microsoft.Extensions.Hosting.Wpf
{
    /// <summary>
    /// Extension methods to configure Wpf
    /// </summary>
    public static class WpfBuilderExtensions 
    {
        /// <summary>
        /// Register a type for the main window
        /// </summary>
        /// <typeparam name="TMainWindow">Type of the main window, must inherit from Window</typeparam>
        /// <param name="wpfBuilder">IWpfBuilder</param>
        /// <returns>IWpfBuilder</returns>
        public static IWpfBuilder UseMainWindow<TMainWindow>(this IWpfBuilder wpfBuilder) where TMainWindow : Window  {
            wpfBuilder.MainWindowType = typeof(TMainWindow);
            return wpfBuilder;
        }

        /// <summary>
        /// Register a type for the main window
        /// </summary>
        /// <typeparam name="TApplication">Type of the application, must inherit from Application</typeparam>
        /// <param name="wpfBuilder">IWpfBuilder</param>
        /// <returns>IWpfBuilder</returns>
        public static IWpfBuilder UseApplication<TApplication>(this IWpfBuilder wpfBuilder) where TApplication : Application
        {
            wpfBuilder.ApplicationType = typeof(TApplication);
            return wpfBuilder;
        }

        /// <summary>
        /// Register action to configure the Application
        /// </summary>
        /// <param name="wpfBuilder">IWpfBuilder</param>
        /// <param name="configureAction">Action to configure the Application</param>
        /// <returns>IWpfBuilder</returns>
        public static IWpfBuilder ConfigureContext(this IWpfBuilder wpfBuilder, Action<IWpfContext> configureAction)
        {
            wpfBuilder.ConfigureContextAction = configureAction;
            return wpfBuilder;
        }
    }
}
