using System;
using System.Reflection;
using System.Runtime.Loader;

namespace Dapplo.Microsoft.Extensions.Hosting.Plugins.Internals
{
    /// <summary>
    /// This AssemblyLoadContext uses an AssemblyDependencyResolver as described here: https://devblogs.microsoft.com/dotnet/announcing-net-core-3-preview-3/
    /// Before loading an assembly, the current domain is checked if this assembly was not already loaded, if so this is returned.
    /// This way the Assemblies already loaded by the application are available to all the plugins and can provide interaction.
    /// </summary>
    internal class PluginLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;

        public PluginLoadContext(string pluginPath)
        {
            _resolver = new AssemblyDependencyResolver(pluginPath);
        }

        /// <inheritdoc />
        protected override Assembly Load(AssemblyName assemblyName)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.Equals(assemblyName.FullName))
                {
                    return assembly;
                }
            }
            var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath == null)
            {
                return null;
            }

            return LoadFromAssemblyPath(assemblyPath);
        }

        /// <inheritdoc />
        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            var libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath == null)
            {
                return IntPtr.Zero;
            }

            return LoadUnmanagedDllFromPath(libraryPath);
        }
    }
}
