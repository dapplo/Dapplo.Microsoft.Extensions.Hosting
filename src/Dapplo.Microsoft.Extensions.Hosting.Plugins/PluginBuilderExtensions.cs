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

namespace Dapplo.Microsoft.Extensions.Hosting.Plugins
{
    /// <summary>
    /// 
    /// </summary>
    public static class PluginBuilderExtensions
    {
        /// <summary>
        /// Add a directory to scan both framework and plug-in assemblies
        /// </summary>
        /// <param name="pluginBuilder">IPluginBuilder</param>
        /// <param name="directories">string array</param>
        public static void AddScanDirectories(this IPluginBuilder pluginBuilder, params string[] directories)
        {
            foreach (var directory in directories)
            {
                pluginBuilder.FrameworkDirectories.Add(directory);
                pluginBuilder.PluginDirectories.Add(directory);
            }
        }

        /// <summary>
        /// Exclude globs to look for framework assemblies
        /// </summary>
        /// <param name="pluginBuilder">IPluginBuilder</param>
        /// <param name="frameworkGlobs">string array</param>
        public static void ExcludeFrameworks(this IPluginBuilder pluginBuilder, params string[] frameworkGlobs)
        {
            foreach (var glob in frameworkGlobs)
            {
                pluginBuilder.FrameworkMatcher.AddExclude(glob);
            }
        }

        /// <summary>
        /// Exclude globs to look for plug-in assemblies
        /// </summary>
        /// <param name="pluginBuilder">IPluginBuilder</param>
        /// <param name="pluginGlobs">string array</param>
        public static void ExcludePlugins(this IPluginBuilder pluginBuilder, params string[] pluginGlobs)
        {
            foreach (var glob in pluginGlobs)
            {
                pluginBuilder.PluginMatcher.AddExclude(glob);
            }
        }

        /// <summary>
        /// Include globs to look for framework assemblies
        /// </summary>
        /// <param name="pluginBuilder">IPluginBuilder</param>
        /// <param name="frameworkGlobs">string array</param>
        public static void IncludeFrameworks(this IPluginBuilder pluginBuilder, params string[] frameworkGlobs)
        {
            foreach (var glob in frameworkGlobs)
            {
                pluginBuilder.PluginMatcher.AddInclude(glob);
            }
        }

        /// <summary>
        /// Include globs to look for plugin assemblies
        /// </summary>
        /// <param name="pluginBuilder">IPluginBuilder</param>
        /// <param name="pluginGlobs">string array</param>
        public static void IncludePlugins(this IPluginBuilder pluginBuilder, params string[] pluginGlobs)
        {
            foreach (var glob in pluginGlobs)
            {
                pluginBuilder.PluginMatcher.AddInclude(glob);
            }
        }

    }
}
