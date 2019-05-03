// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Threading;

namespace Dapplo.Microsoft.Extensions.Hosting.WinForms.Internals
{
    /// <inheritdoc />
    public class WinFormsContext : IWinFormsContext
    {
        private readonly WinFormsThread _winFormsThread;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public WinFormsContext()
        {
            _winFormsThread = new WinFormsThread(this);
        }
        
        /// <inheritdoc />
        public void StartUi(IServiceProvider serviceProvider)
        {
            _winFormsThread.Start(serviceProvider);
        }
        
        /// <inheritdoc />
        public bool IsLifetimeLinked { get; set; }

        /// <inheritdoc />
        public bool EnableVisualStyles { get; set; } = true;

        /// <inheritdoc />
        public bool IsRunning { get; set; }

        /// <inheritdoc />
        public Dispatcher Dispatcher { get; set; }
    }
}