using System;
using System.Collections.Generic;
using System.Text;
using Dapplo.Microsoft.Extensions.Hosting.AppServices.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.AppServices;

internal static class InternalBuilderAppServiceUtility
{
    private const string MutexBuilderKey = "MutexBuilder";

    /// <summary>
    /// Helper method to retrieve the mutex builder
    /// </summary>
    /// <param name="properties">IDictionary</param>
    /// <param name="mutexBuilder">IMutexBuilder out value</param>
    /// <returns>bool if there was a matcher</returns>
    private static bool TryRetrieveMutexBuilder(IDictionary<object, object> properties, out IMutexBuilder mutexBuilder)
    {
        if (properties.TryGetValue(MutexBuilderKey, out var mutexBuilderObject))
        {
            mutexBuilder = (IMutexBuilder)mutexBuilderObject;
            return true;

        }
        mutexBuilder = new MutexBuilder();
        properties[MutexBuilderKey] = mutexBuilder;
        return false;
    }

    /// <summary>
    /// Prevent that an application runs multiple times
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <param name="configureAction">Action to configure IMutexBuilder</param>
    /// <returns>IHostApplicationBuilder for fluently calling</returns>
    internal static IHostApplicationBuilder ConfigureSingleInstance(IHostApplicationBuilder hostApplicationBuilder, Action<IMutexBuilder> configureAction)
    {
        InnerConfigureSingleInstance(hostApplicationBuilder.Properties, hostApplicationBuilder.Services, configureAction);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// Prevent that an application runs multiple times
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <param name="configureAction">Action to configure IMutexBuilder</param>
    /// <returns>IHostBuilder for fluently calling</returns>
    internal static IHostBuilder ConfigureSingleInstance(IHostBuilder hostBuilder, Action<IMutexBuilder> configureAction) =>
        hostBuilder.ConfigureServices((hostContext, serviceCollection) =>
            InnerConfigureSingleInstance(hostBuilder.Properties, serviceCollection, configureAction));

    private static void InnerConfigureSingleInstance(IDictionary<object, object> properties, IServiceCollection serviceCollection, Action<IMutexBuilder> configureAction)
    {
        if (!TryRetrieveMutexBuilder(properties, out var mutexBuilder))
        {
            serviceCollection
                .AddSingleton(mutexBuilder)
                .AddHostedService<MutexLifetimeService>();
        }
        configureAction?.Invoke(mutexBuilder);
    }

    /// <summary>
    /// Prevent that an application runs multiple times
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <param name="mutexId">string</param>
    /// <returns>IHostApplicationBuilder for fluently calling</returns>
    internal static IHostApplicationBuilder ConfigureSingleInstance(IHostApplicationBuilder hostApplicationBuilder, string mutexId) =>    
        ConfigureSingleInstance(hostApplicationBuilder, builder => builder.MutexId = mutexId);


    /// <summary>
    /// Prevent that an application runs multiple times
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <param name="mutexId">string</param>
    /// <returns>IHostBuilder for fluently calling</returns>
    internal static IHostBuilder ConfigureSingleInstance(IHostBuilder hostBuilder, string mutexId) =>    
        ConfigureSingleInstance(hostBuilder, builder => builder.MutexId = mutexId);
}
