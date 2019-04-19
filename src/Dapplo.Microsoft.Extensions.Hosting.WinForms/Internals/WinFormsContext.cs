// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows.Threading;

namespace Dapplo.Microsoft.Extensions.Hosting.WinForms.Internals
{
    /// <inheritdoc />
    public class WinFormsContext : IWinFormsContext
    {
        /// <inheritdoc />
        public bool IsLifetimeLinked { get; set; }

        /// <inheritdoc />
        public bool EnableVisualStyles { get; set; } = true;

        /// <inheritdoc />
        public bool IsRunning { get; set; }

        /// <inheritdoc />
        public Dispatcher FormsDispatcher { get; set; }
    }
}