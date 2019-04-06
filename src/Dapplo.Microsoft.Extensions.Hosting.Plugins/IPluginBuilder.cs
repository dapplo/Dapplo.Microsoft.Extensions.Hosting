//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2019 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Microsoft.Extensions.Hosting
// 
//  Dapplo.Microsoft.Extensions.Hosting is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Microsoft.Extensions.Hosting is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Microsoft.Extensions.Hosting. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

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
