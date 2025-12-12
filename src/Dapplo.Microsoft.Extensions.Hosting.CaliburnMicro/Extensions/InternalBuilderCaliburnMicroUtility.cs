using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Dapplo.Microsoft.Extensions.Hosting.CaliburnMicro.Internals;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.CaliburnMicro.Extensions;

internal static class InternalBuilderCaliburnMicroUtility
{
    private const string CaliburnMicroContextKey = "CaliburnMicroContext";

    /// <summary>
    /// Helper method to retrieve the ICaliburnMicroContext
    /// </summary>
    /// <param name="properties">IDictionary</param>
    /// <param name="caliburnMicroContext">ICaliburnMicroContext</param>
    /// <returns>bool if there was already an </returns>
    private static bool TryRetrieveCaliburnMicroContext(IDictionary<object, object> properties, out ICaliburnMicroContext caliburnMicroContext)
    {
        if (properties.TryGetValue(CaliburnMicroContextKey, out var caliburnContextAsObject))
        {
            caliburnMicroContext = (ICaliburnMicroContext)caliburnContextAsObject;
            return true;

        }
        caliburnMicroContext = new CaliburnMicroContext();
        properties[CaliburnMicroContextKey] = caliburnMicroContext;
        return false;
    }

    /// <summary>
    /// Configure Caliburn.Micro
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <returns>IHostBuilder</returns>
    internal static IHostApplicationBuilder ConfigureCaliburnMicro(IHostApplicationBuilder hostApplicationBuilder)
    {
        InnerConfigureCaliburnMicro(hostApplicationBuilder.Properties, hostApplicationBuilder.Services);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// Configure Caliburn.Micro
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <returns>IHostBuilder</returns>
    internal static IHostBuilder ConfigureCaliburnMicro(IHostBuilder hostBuilder) =>     
        hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) => InnerConfigureCaliburnMicro(hostBuilder.Properties, serviceCollection));        

    private static void InnerConfigureCaliburnMicro(IDictionary<object, object> properties, IServiceCollection serviceCollection)
    {
        if (!TryRetrieveCaliburnMicroContext(properties, out var caliburnMicroContext))
        {
            serviceCollection
                .AddSingleton(caliburnMicroContext)
                .AddSingleton<IWindowManager, CaliburnMicroWindowManager>()
                .AddSingleton<IWpfService, CaliburnMicroBootstrapper>();
        }
    }

    /// <summary>
    /// Configure Caliburn.Micro with the shell
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <returns>IHostApplicationBuilder</returns>
    internal static IHostApplicationBuilder ConfigureCaliburnMicro<TShell>(IHostApplicationBuilder hostApplicationBuilder) where TShell : class, ICaliburnMicroShell
    {
        InnerConfigureCaliburnMicro<TShell>(hostApplicationBuilder.Properties, hostApplicationBuilder.Services);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// Configure Caliburn.Micro with the shell
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <returns>IHostBuilder</returns>
    internal static IHostBuilder ConfigureCaliburnMicro<TShell>(IHostBuilder hostBuilder)
        where TShell : class, ICaliburnMicroShell =>
        hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) => InnerConfigureCaliburnMicro<TShell>(hostBuilder.Properties, serviceCollection));
    

    private static void InnerConfigureCaliburnMicro<TShell>(IDictionary<object, object> properties, IServiceCollection serviceCollection)
        where TShell : class, ICaliburnMicroShell
    {
        InnerConfigureCaliburnMicro(properties, serviceCollection);
        serviceCollection.AddSingleton<ICaliburnMicroShell, TShell>();
    }
}
