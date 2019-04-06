using Dapplo.Microsoft.Extensions.Hosting.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Hosting.Sample.PluginOriginalSample
{
    /// <summary>
    /// This plug-in configures the HostBuilderContext to have the hosted services from the online example
    /// </summary>
    [PluginOrder(100)]
    public class Plugin : IPlugin
    {
        /// <inheritdoc />
        public void ConfigureHost(HostBuilderContext hostBuilderContext, IServiceCollection serviceCollection)
        {
            serviceCollection.AddHostedService<LifetimeEventsHostedService>();
            serviceCollection.AddHostedService<TimedHostedService>();
        }
    }
}
