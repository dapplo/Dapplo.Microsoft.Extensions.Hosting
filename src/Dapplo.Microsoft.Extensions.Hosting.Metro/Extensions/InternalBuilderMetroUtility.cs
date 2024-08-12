using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapplo.Microsoft.Extensions.Hosting.Metro.Internals;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.Metro;

internal static class InternalBuilderMetroUtility
{
    private const string MetroContextKey = "MetroContext";

    /// <summary>
    /// Helper method to retrieve the IMetroContext
    /// </summary>
    /// <param name="properties">IDictionary</param>
    /// <param name="metroContext">IMetroContext out value</param>
    /// <returns>bool if there was already an IMetroContext</returns>
    private static bool TryRetrieveMetroContext(IDictionary<object, object> properties, out IMetroContext metroContext)
    {
        if (properties.TryGetValue(MetroContextKey, out var metroContextAsObject))
        {
            metroContext = (IMetroContext)metroContextAsObject;
            return true;

        }
        metroContext = new MetroContext();
        properties[MetroContextKey] = metroContext;
        return false;
    }

    /// <summary>
    /// This enables MahApps.Metro
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <param name="configureAction">Action to configure IMetroContext</param>
    /// <returns>IHostApplicationBuilder</returns>
    internal static IHostApplicationBuilder ConfigureMetro(IHostApplicationBuilder hostApplicationBuilder, Action<IMetroContext> configureAction = null)
    {
        InnerConfigureMetro(hostApplicationBuilder.Properties, hostApplicationBuilder.Services, configureAction);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// This enables MahApps.Metro
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <param name="configureAction">Action to configure IMetroContext</param>
    /// <returns>IHostBuilder</returns>
    internal static IHostBuilder ConfigureMetro(IHostBuilder hostBuilder, Action<IMetroContext> configureAction = null) =>
        hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) => InnerConfigureMetro(hostBuilder.Properties, serviceCollection, configureAction));

    /// <summary>
    /// Configure WPF to use MahApps.Metro and specify the theme
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <param name="theme">string</param>
    /// <returns>IHostApplicationBuilder</returns>
    internal static IHostApplicationBuilder ConfigureMetro(IHostApplicationBuilder hostApplicationBuilder, string theme)
    {
        InnerConfigureMetro(hostApplicationBuilder.Properties, hostApplicationBuilder.Services, context => context.Theme = theme);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// Configure WPF to use MahApps.Metro and specify the theme
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <param name="theme">string</param>
    /// <returns>IHostBuilder</returns>
    internal static IHostBuilder ConfigureMetro(IHostBuilder hostBuilder, string theme) =>
        hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) => InnerConfigureMetro(hostBuilder.Properties, serviceCollection, context => context.Theme = theme));

    private static void InnerConfigureMetro(IDictionary<object, object> properties, IServiceCollection serviceCollection, Action<IMetroContext> configureAction)
    {
        if (!TryRetrieveMetroContext(properties, out var metroContext))
        {
            serviceCollection.AddSingleton(metroContext);
            serviceCollection.AddSingleton<IWpfService, MetroWpfService>();
        }
        // Configure the default styles
        metroContext.Styles.AddRange(new[] { "Controls", "Fonts" });
        configureAction?.Invoke(metroContext);
    }
}
