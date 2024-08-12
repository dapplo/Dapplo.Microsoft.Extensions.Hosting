using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Microsoft.Extensions.DependencyInjection;

namespace Dapplo.Microsoft.Extensions.Hosting.ReactiveUI.Wpf;

internal static class InternalBuilderReactiveUiUtility
{
    /// <summary>
    /// Configure a ReactiveUI application.
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder.</param>
    /// <returns>IHostApplicationBuilder</returns>
    internal static IHostApplicationBuilder ConfigureSplatForMicrosoftDependencyResolver(IHostApplicationBuilder hostApplicationBuilder)
    {
        InnerConfigureSplatForMicrosoftDependencyResolver(hostApplicationBuilder.Services);
        return hostApplicationBuilder;
    }

    /// <summary>
    /// Configure a ReactiveUI application.
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder.</param>
    /// <returns>I Host Builder.</returns>
    internal static IHostBuilder ConfigureSplatForMicrosoftDependencyResolver(this IHostBuilder hostBuilder) =>    
        hostBuilder.ConfigureServices((serviceCollection) => InnerConfigureSplatForMicrosoftDependencyResolver(serviceCollection));
       

    private static void InnerConfigureSplatForMicrosoftDependencyResolver(IServiceCollection serviceCollection)
    {
        serviceCollection.UseMicrosoftDependencyResolver();
        var resolver = Locator.CurrentMutable;
        resolver.InitializeSplat();
        resolver.InitializeReactiveUI();
    }
}
