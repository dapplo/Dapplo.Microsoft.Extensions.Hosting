// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dapplo.Microsoft.Extensions.Hosting.UiThread
{
    /// <inheritdoc />
    public abstract class UiContext : IUiContext
    {
        /// <inheritdoc />
        public bool IsLifetimeLinked { get; set; }

        /// <inheritdoc />
        public bool IsRunning { get; set; }
    }
}