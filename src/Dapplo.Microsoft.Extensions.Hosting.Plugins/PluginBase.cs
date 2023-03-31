using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.Plugins
{
    /// <summary>
    /// Plugin Base.
    /// </summary>
    /// <typeparam name="T">The type of Plugin.</typeparam>
    /// <seealso cref="IPlugin" />
    public class PluginBase<T> : IPlugin
        where T : class, IHostedService
    {
        /// <inheritdoc />
        public void ConfigureHost(HostBuilderContext hostBuilderContext, IServiceCollection serviceCollection) =>
            serviceCollection.AddHostedService<T>();
    }
}
