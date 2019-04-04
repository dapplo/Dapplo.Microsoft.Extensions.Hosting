using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private static readonly Matcher GlobalMatcher = new Matcher();
        private static bool _isPluginLoadingConfigured;
        
        /// <summary>
        /// This enables scanning for and loading of plug-ins
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="configureAction">Action to configure where the plugins come from</param>
        /// <returns>IHostBuilder for fluently calling</returns>
        public static IHostBuilder AddPlugins(this IHostBuilder hostBuilder, Action<Matcher> configureAction)
        {
            configureAction?.Invoke(GlobalMatcher);
            ConfigurePluginScanAndLoad(hostBuilder);
            return hostBuilder;
        }

        /// <summary>
        /// This enables scanning for and loading of plug-ins
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="globs">The glob patterns to match plugins with, they are located from the ContentRoot</param>
        /// <returns>IHostBuilder for fluently calling</returns>
        public static IHostBuilder AddPlugins(this IHostBuilder hostBuilder, params string[] globs)
        {
            GlobalMatcher.AddIncludePatterns(globs);
            ConfigurePluginScanAndLoad(hostBuilder);
            return hostBuilder;
        }

        /// <summary>
        /// This enables scanning for and loading of plug-ins
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        private static void ConfigurePluginScanAndLoad(IHostBuilder hostBuilder)
        {
            // Check if we need to do something
            if (_isPluginLoadingConfigured)
            {
                return;
            }
            _isPluginLoadingConfigured = true;
            // Configure the actual scanning & loading
            hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                // Figure out the root from which we scan
                var scanRoot = hostBuilderContext.HostingEnvironment.ContentRootPath ?? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                // Do the globbing and try to load the plug-ins
                foreach (var pluginPath in GlobalMatcher.GetResultsInFullPath(scanRoot))
                {
                    var plugin = LoadPlugin(pluginPath);
                    plugin?.ConfigureHost(hostBuilderContext, serviceCollection);
                }
            });
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
                Console.WriteLine($"Can't find: {pluginLocation}");
                return null;
            }
            Console.WriteLine($"Loading plugin from: {pluginLocation}");
 
            var loadContext = new PluginLoadContext(pluginLocation);
            var assembly = loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));

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
