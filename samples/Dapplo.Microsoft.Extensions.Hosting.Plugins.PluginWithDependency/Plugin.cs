using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.Plugins.PluginWithDependency
{
    /// <summary>
    /// A simple plugin definition, registering the BackgroundService
    /// </summary>
    public class Plugin : IPlugin
    {
        /// <inheritdoc />
        public void ConfigureHost(HostBuilderContext hostBuilderContext, IServiceCollection serviceCollection)
        {
            serviceCollection.AddHostedService<BackgroundService>();
        }
    }
}
