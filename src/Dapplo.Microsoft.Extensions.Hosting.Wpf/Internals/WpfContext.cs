// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Threading;

namespace Dapplo.Microsoft.Extensions.Hosting.Wpf.Internals
{
    /// <inheritdoc />
    internal class WpfContext : IWpfContext
    {
        private readonly WpfThread _wpfThread;

        /// <summary>
        /// Default constructor
        /// </summary>
        public WpfContext()
        {
            _wpfThread = new WpfThread(this);
        }

        /// <inheritdoc />
        public void StartUi(IServiceProvider serviceProvider)
        {
            _wpfThread.Start(serviceProvider);
        }
        
        /// <inheritdoc />
        public ShutdownMode ShutdownMode { get; set; } = ShutdownMode.OnLastWindowClose;

        /// <inheritdoc />
        public bool IsLifetimeLinked { get; set; }

        /// <inheritdoc />
        public bool IsRunning { get; set; }

        /// <inheritdoc />
        public Application WpfApplication { get; set; }
        
        /// <inheritdoc />
        public Dispatcher Dispatcher => WpfApplication.Dispatcher;
    }
}