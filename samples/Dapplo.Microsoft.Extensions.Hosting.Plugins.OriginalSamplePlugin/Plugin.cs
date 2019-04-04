using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.Plugins.OriginalSamplePlugin
{
    /// <summary>
    /// Init class for the plugin
    /// </summary>
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
