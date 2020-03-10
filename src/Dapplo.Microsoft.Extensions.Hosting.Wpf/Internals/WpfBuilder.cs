// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dapplo.Microsoft.Extensions.Hosting.Wpf.Internals
{
    /// <inheritdoc/>
    internal class WpfBuilder : IWpfBuilder
    {
        public Type ApplicationType { get; set; }
        public Type MainWindowType { get; set; }
        public Action<IWpfContext> ConfigureContextAction { get; set; }
    }
}