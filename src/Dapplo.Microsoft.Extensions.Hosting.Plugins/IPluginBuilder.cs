// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.FileSystemGlobbing;
using System.Collections.Generic;

namespace Dapplo.Microsoft.Extensions.Hosting.Plugins
{
    /// <summary>
    /// The plug-in builder is used to configure the plug-in loading.
    /// </summary>
    public interface IPluginBuilder
    {
        /// <summary>
        /// In these directories we will scan for plug-ins
        /// </summary>
        IList<string> PluginDirectories { get; }

        /// <summary>
        /// In these directories we will scan for framework assemblies
        /// </summary>
        IList<string> FrameworkDirectories { get; }

        /// <summary>
        /// Specify to use the content root for scanning
        /// </summary>
        bool UseContentRoot { get; set; }

        /// <summary>
        /// The matcher used to find all the framework assemblies
        /// </summary>
        Matcher FrameworkMatcher { get; }

        /// <summary>
        /// The matcher to find all the plugins
        /// </summary>
        Matcher PluginMatcher { get; }
    }
}
