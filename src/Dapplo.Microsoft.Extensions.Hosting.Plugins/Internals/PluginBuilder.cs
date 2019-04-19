// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.Extensions.FileSystemGlobbing;

namespace Dapplo.Microsoft.Extensions.Hosting.Plugins.Internals
{
    /// <summary>
    /// This is the implementation of the plugin builder
    /// </summary>
    internal class PluginBuilder : IPluginBuilder
    {
        /// <inheritdoc />
        public Matcher FrameworkMatcher { get; } = new Matcher();

        /// <inheritdoc />
        public Matcher PluginMatcher { get; } = new Matcher();

        /// <inheritdoc />
        public IList<string> PluginDirectories { get; } = new List<string>();

        /// <inheritdoc />
        public IList<string> FrameworkDirectories { get; } = new List<string>();

        /// <inheritdoc />
        public bool UseContentRoot { get; set; }
    }
}
