// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Dapplo.Microsoft.Extensions.Hosting.WinUI.Internals
{
    /// <inheritdoc />
    public class WinUIBuilder : IWinUIBuilder
    {
        /// <inheritdoc/>
        public Action<IWinUIContext> ConfigureContextAction { get; set; }

        /// <inheritdoc/>
        public IList<Type> WindowTypes { get; } = new List<Type>();
    }
}
