// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Threading;

namespace Dapplo.Microsoft.Extensions.Hosting.Wpf
{
    /// <summary>
    /// The WPF context contains all information about the WPF application and how it's started and stopped
    /// </summary>
    public interface IWpfContext
    {
        /// <summary>
        /// This is the WPF ShutdownMode used for the WPF application lifetime, default is OnLastWindowClose
        /// </summary>
        ShutdownMode ShutdownMode { get; set; }
        
        /// <summary>
        /// Defines if the host application is stopped when the WPF applications stops
        /// </summary>
        bool IsLifetimeLinked { get; set; }
        
        /// <summary>
        /// Is the WPF application started and still running? 
        /// </summary>
        bool IsRunning { get; set; }
        
        /// <summary>
        /// The Application
        /// </summary>
        Application WpfApplication { get; set; }

        /// <summary>
        /// This starts the UI Thread logic
        /// </summary>
        void StartUi(IServiceProvider serviceProvider);

        /// <summary>
        /// This WPF dispatcher
        /// </summary>
        Dispatcher Dispatcher { get; } 
    }
}