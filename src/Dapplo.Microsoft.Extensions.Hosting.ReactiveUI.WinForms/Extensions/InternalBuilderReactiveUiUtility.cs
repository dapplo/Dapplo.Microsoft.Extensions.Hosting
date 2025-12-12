using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace Dapplo.Microsoft.Extensions.Hosting.ReactiveUI
{
    internal static class InternalBuilderReactiveUiUtility
    {
        /// <summary>
        /// Configure a ReactiveUI application.
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder.</param>
        /// <returns>I Host Builder.</returns>
        internal static IHostBuilder ConfigureSplatForMicrosoftDependencyResolver(IHostBuilder hostBuilder) =>        
            hostBuilder.ConfigureServices(serviceCollection => InnerConfigureSplatForMicrosoftDependencyResolver(serviceCollection));

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

        private static void InnerConfigureSplatForMicrosoftDependencyResolver(IServiceCollection serviceCollection)
        {
            serviceCollection.UseMicrosoftDependencyResolver();
            var resolver = Locator.CurrentMutable;
            resolver.InitializeSplat();
            resolver.InitializeReactiveUI();
        }
    }
}
