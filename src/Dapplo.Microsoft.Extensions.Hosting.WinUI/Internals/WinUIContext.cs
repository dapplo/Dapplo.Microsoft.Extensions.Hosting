// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;

namespace Dapplo.Microsoft.Extensions.Hosting.WinUI.Internals
{
    /// <inheritdoc />
    public class WinUIContext : IWinUIContext
    {
        /// <inheritdoc />
        public DispatcherQueue Dispatcher { get; set; }

        /// <inheritdoc />
        public bool IsLifetimeLinked { get; set; }

        /// <inheritdoc />
        public bool IsRunning { get; set; }

        /// <inheritdoc />
        public IList<Type> WindowTypes { get; } = new List<Type>();

        /// <inheritdoc />
        public Application WinUIApplication { get; set; }
    }
}