using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapplo.Microsoft.Extensions.Hosting.WinUI.Internals;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace Dapplo.Microsoft.Extensions.Hosting.WinUI;

internal static class InternalBuilderWinUIUtility
{
    private const string WinUIContextKey = "WinUIContext";

    /// <summary>
    /// Helper method to retrieve the IWinUIContext
    /// </summary>
    /// <param name="properties">IDictionary</param>
    /// <param name="winUIContext">IWinUIContext out value</param>
    /// <returns>bool if there was already an IWinUIContext</returns>
    private static bool TryRetrieveWinUIContext(IDictionary<object, object> properties, out IWinUIContext winUIContext)
    {
        if (properties.TryGetValue(WinUIContextKey, out var winUIContextAsObject))
        {
            winUIContext = (IWinUIContext)winUIContextAsObject;
            return true;
        }
        winUIContext = new WinUIContext();
        properties[WinUIContextKey] = winUIContext;
        return false;
    }

    /// <summary>
    /// Configure a WinUI application
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <returns></returns>
    internal static IHostApplicationBuilder ConfigureWinUI<TApp, TAppWindow>(IHostApplicationBuilder hostApplicationBuilder)
        where TApp : Application
        where TAppWindow : Window
    {
        InnerConfigureWinUI<TApp, TAppWindow>(hostApplicationBuilder.Properties, hostApplicationBuilder.Services);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// Configure a WinUI application
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <returns></returns>
    internal static IHostBuilder ConfigureWinUI<TApp, TAppWindow>(IHostBuilder hostBuilder)
        where TApp : Application
        where TAppWindow : Window =>    
        hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) => InnerConfigureWinUI<TApp, TAppWindow>(hostBuilder.Properties, serviceCollection));

    private static void InnerConfigureWinUI<TApp, TAppWindow>(IDictionary<object, object> properties, IServiceCollection serviceCollection)
        where TApp : Application
        where TAppWindow : Window
    {
        var appType = typeof(TApp);


        if (!TryRetrieveWinUIContext(properties, out var winUIContext))
        {
            serviceCollection.AddSingleton(winUIContext);
            serviceCollection.AddSingleton(serviceProvider => new WinUIThread(serviceProvider));
            serviceCollection.AddHostedService<WinUIHostedService>();
        }

        winUIContext.AppWindowType = typeof(TAppWindow);
        winUIContext.IsLifetimeLinked = true;


        if (appType != null)
        {
            var baseApplicationType = typeof(Application);
            if (!baseApplicationType.IsAssignableFrom(appType))
            {
                throw new ArgumentException("The registered Application type inherit System.Windows.Application", nameof(TApp));
            }

            serviceCollection.AddSingleton<TApp>();

            if (appType != baseApplicationType)
            {
                serviceCollection.AddSingleton<Application>(services => services.GetRequiredService<TApp>());
            }
        }
    }
}
