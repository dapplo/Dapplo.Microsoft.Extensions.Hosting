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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Dapplo.Microsoft.Extensions.Hosting.Plugins.Internals;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.Plugins
{
    /// <summary>
    /// Extensions for adding plug-ins to your host
    /// </summary>
    public static class HostBuilderPluginExtensions
    {
        private const string PluginMatcherKey = "PluginMatcher";
        private const string FrameworkMatcherKey = "FrameworkMatcher";

        /// <summary>
        /// Helper method to retrieve a matcher
        /// </summary>
        /// <param name="properties">IDictionary</param>
        /// <param name="propertyName">string</param>
        /// <param name="matcher">Matcher out value</param>
        /// <param name="create">bool specifying if a new Matcher needs to be created</param>
        /// <returns>bool if there was a matcher</returns>
        private static bool TryGetMatcher(this IDictionary<object, object> properties, string propertyName, out Matcher matcher, bool create = true)
        {
            if (properties.TryGetValue(propertyName, out var matcherObject))
            {
                matcher = matcherObject as Matcher;
                return true;
            }
            if (create)
            {
                matcher = new Matcher();
                properties[propertyName] = matcher;
            } else
            {
                matcher = null;
            }
            return false;
        }

        /// <summary>
        /// Specify what assemblies to load into the host, this is needed for frameworks used over multiple plug-ins
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="configureAction">Action to configure where the framework assemblies come from</param>
        /// <returns>IHostBuilder for fluently calling</returns>
        public static IHostBuilder AddFrameworkAssemblies(this IHostBuilder hostBuilder, Action<Matcher> configureAction)
        {
            if (!hostBuilder.Properties.TryGetMatcher(FrameworkMatcherKey, out var frameworkMatcher) && !hostBuilder.Properties.TryGetMatcher(PluginMatcherKey, out var pluginMatcher))
            {
                ConfigurePluginScanAndLoad(hostBuilder);
            }
            configureAction?.Invoke(frameworkMatcher);
            return hostBuilder;
        }

        /// <summary>
        /// Specify what assemblies to load into the host, this is needed for frameworks used over multiple plug-ins
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="globs">The glob patterns to match framework assemblies with, they are located from the ContentRoot</param>
        /// <returns>IHostBuilder for fluently calling</returns>
        public static IHostBuilder AddFrameworkAssemblies(this IHostBuilder hostBuilder, params string[] globs)
        {
            if (!hostBuilder.Properties.TryGetMatcher(FrameworkMatcherKey, out var frameworkMatcher) && !hostBuilder.Properties.TryGetMatcher(PluginMatcherKey, out var pluginMatcher))
            {
                ConfigurePluginScanAndLoad(hostBuilder);
            }
            frameworkMatcher.AddIncludePatterns(globs);
            return hostBuilder;
        }
        
        /// <summary>
        /// Specify what plug-ins assemblies to load
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="configureAction">Action to configure where the plugins come from</param>
        /// <returns>IHostBuilder for fluently calling</returns>
        public static IHostBuilder AddPluginAssemblies(this IHostBuilder hostBuilder, Action<Matcher> configureAction)
        {
            if (!hostBuilder.Properties.TryGetMatcher(PluginMatcherKey, out var pluginMatcher) && !hostBuilder.Properties.TryGetMatcher(FrameworkMatcherKey, out var frameworkMatcher))
            {
                ConfigurePluginScanAndLoad(hostBuilder);
            }
            configureAction?.Invoke(pluginMatcher);
            return hostBuilder;
        }

        /// <summary>
        /// Specify what plug-ins to load
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="globs">The glob patterns to match plug-in assemblies, they are located from the ContentRoot</param>
        /// <returns>IHostBuilder for fluently calling</returns>
        public static IHostBuilder AddPluginAssemblies(this IHostBuilder hostBuilder, params string[] globs)
        {
            if (!hostBuilder.Properties.TryGetMatcher(PluginMatcherKey, out var pluginMatcher) && !hostBuilder.Properties.TryGetMatcher(FrameworkMatcherKey, out var frameworkMatcher))
            {
                ConfigurePluginScanAndLoad(hostBuilder);
            }
            pluginMatcher.AddIncludePatterns(globs);
            return hostBuilder;
        }

        /// <summary>
        /// This enables scanning for and loading of plug-ins
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        private static void ConfigurePluginScanAndLoad(IHostBuilder hostBuilder)
        {
            // Configure the actual scanning & loading
            hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                // Figure out the root from which we scan
                var scanRoot = hostBuilderContext.HostingEnvironment.ContentRootPath ?? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (hostBuilderContext.Properties.TryGetMatcher(FrameworkMatcherKey, out var frameworkMatcher, false))
                {
                    // Do the globbing and try to load the framework files into the default AssemblyLoadContext
                    foreach (var frameworkAssemblyPath in frameworkMatcher.GetResultsInFullPath(scanRoot))
                    {
                        var frameworkAssemblyName = new AssemblyName(Path.GetFileNameWithoutExtension(frameworkAssemblyPath));
                        if (AssemblyLoadContext.Default.TryGetAssembly(frameworkAssemblyName, out _)) 
                        {
                            continue;
                        }
                    
                        // TODO: Log the loading?
                        AssemblyLoadContext.Default.LoadFromAssemblyPath(frameworkAssemblyPath);
                    }
                }

                if (hostBuilderContext.Properties.TryGetMatcher(PluginMatcherKey, out var pluginMatcher, false))
                {
                    // Do the globbing and try to load the plug-ins
                    var pluginPaths = pluginMatcher.GetResultsInFullPath(scanRoot);
                    var plugins = pluginPaths
                        .Select(pluginPath => LoadPlugin(pluginPath))
                        .Where(plugin => plugin != null)
                        .OrderBy(plugin => plugin.GetOrder());
                    foreach (var plugin in plugins)
                    {
                        plugin?.ConfigureHost(hostBuilderContext, serviceCollection);
                    }
                }
            });
        }

        /// <summary>
        /// Helper method to process the PluginOrder attribute
        /// </summary>
        /// <param name="plugin">IPlugin</param>
        /// <returns>int</returns>
        private static int GetOrder(this IPlugin plugin)
        {
            return plugin.GetType().GetCustomAttribute<PluginOrderAttribute>()?.Order ?? 0;
        }

        /// <summary>
        /// Helper method to load an assembly which contains a single plugin
        /// </summary>
        /// <param name="pluginLocation">string</param>
        /// <returns>IPlugin</returns>
        private static IPlugin LoadPlugin(string pluginLocation)
        {
            if (!File.Exists(pluginLocation))
            {
                // TODO: Log an error, how to get a logger here?
                return null;
            }
            
            // TODO: Log verbose that we are loading a plugin
            var pluginName = Path.GetFileNameWithoutExtension(pluginLocation);
            // TODO: Decide if we rather have this to come up with the name: AssemblyName.GetAssemblyName(pluginLocation)
            var pluginAssemblyName = new AssemblyName(pluginName);
            if (AssemblyLoadContext.Default.TryGetAssembly(pluginAssemblyName, out _))
            {
                return null;
            }
            var loadContext = new PluginLoadContext(pluginLocation, pluginName);
            var assembly = loadContext.LoadFromAssemblyName(pluginAssemblyName);

            // TODO: Check if we want to scan all assemblies, or have a specify class on a predetermined location?
            var interfaceType = typeof(IPlugin);
            foreach (var type in assembly.GetExportedTypes())
            {
                if (!type.GetInterfaces().Contains(interfaceType))
                {
                    continue;
                }
                var plugin = Activator.CreateInstance(type) as IPlugin;
                return plugin;
            }
            return null;
        }
    }
}
