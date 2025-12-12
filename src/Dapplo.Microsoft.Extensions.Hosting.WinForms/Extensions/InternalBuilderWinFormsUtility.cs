using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapplo.Microsoft.Extensions.Hosting.WinForms.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.WinForms;

internal static class InternalBuilderWinFormsUtility
{
    private const string WinFormsContextKey = "WinFormsContext";

    /// <summary>
    /// Helper method to retrieve the IWinFormsContext
    /// </summary>
    /// <param name="properties">IDictionary</param>
    /// <param name="winFormsContext">IWinFormsContext out value</param>
    /// <returns>bool if there was already an IWinFormsContext</returns>
    private static bool TryRetrieveWinFormsContext(IDictionary<object, object> properties, out IWinFormsContext winFormsContext)
    {
        if (properties.TryGetValue(WinFormsContextKey, out var winFormsContextAsObject))
        {
            winFormsContext = (IWinFormsContext)winFormsContextAsObject;
            return true;

        }
        winFormsContext = new WinFormsContext();
        properties[WinFormsContextKey] = winFormsContext;
        return false;
    }

    /// <summary>
    /// Defines that stopping the WinForms application also stops the host (application)
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <returns>IHostApplicationBuilder</returns>
    internal static IHostApplicationBuilder UseWinFormsLifetime(IHostApplicationBuilder hostApplicationBuilder)
    {
        InnerUseWinFormsLifetime(hostApplicationBuilder.Properties);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// Defines that stopping the WinForms application also stops the host (application)
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <returns>IHostBuilder</returns>
    internal static IHostBuilder UseWinFormsLifetime(IHostBuilder hostBuilder) =>    
        hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) => InnerUseWinFormsLifetime(hostBuilder.Properties));        

    /// <summary>
    /// Configure an WinForms application
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <param name="configureAction">Action to configure the Application</param>
    /// <returns>IHostApplicationBuilder</returns>
    internal static IHostApplicationBuilder ConfigureWinForms(IHostApplicationBuilder hostApplicationBuilder, Action<IWinFormsContext> configureAction = null)
    {
        InnerConfigureWinForms(hostApplicationBuilder.Properties, hostApplicationBuilder.Services, configureAction);

        return hostApplicationBuilder;
    }


    /// <summary>
    /// Configure an WinForms application
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <param name="configureAction">Action to configure the Application</param>
    /// <returns>IHostBuilder</returns>
    internal static IHostBuilder ConfigureWinForms(IHostBuilder hostBuilder, Action<IWinFormsContext> configureAction = null) =>    
        hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) => InnerConfigureWinForms(hostBuilder.Properties, serviceCollection, configureAction));        

    /// <summary>
    /// Configure an WinForms application
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <param name="configureAction">Action to configure the Application</param>
    /// <typeparam name="TView">Type for the View</typeparam>
    /// <returns>IHostApplicationBuilder</returns>
    internal static IHostApplicationBuilder ConfigureWinForms<TView>(IHostApplicationBuilder hostApplicationBuilder, Action<IWinFormsContext> configureAction = null)
    where TView : Form
    {
        InnerConfigureWinForms<TView>(hostApplicationBuilder.Properties, hostApplicationBuilder.Services, configureAction);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// Configure an WinForms application
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <param name="configureAction">Action to configure the Application</param>
    /// <typeparam name="TView">Type for the View</typeparam>
    /// <returns>IHostBuilder</returns>
    internal static IHostBuilder ConfigureWinForms<TView>(IHostBuilder hostBuilder, Action<IWinFormsContext> configureAction = null)
        where TView : Form =>
        hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) => InnerConfigureWinForms<TView>(hostBuilder.Properties, serviceCollection, configureAction));

    /// <summary>
    /// Specify a shell, the primary Form, to start
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <typeparam name="TShell">Type for the shell, must derive from Form and implement IWinFormsShell</typeparam>
    /// <returns>IHostApplicationBuilder</returns>
    internal static IHostApplicationBuilder ConfigureWinFormsShell<TShell>(IHostApplicationBuilder hostApplicationBuilder)
    where TShell : Form, IWinFormsShell
    {
        InnerConfigureWinForms<TShell>(hostApplicationBuilder.Properties, hostApplicationBuilder.Services);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// Specify a shell, the primary Form, to start
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <typeparam name="TShell">Type for the shell, must derive from Form and implement IWinFormsShell</typeparam>
    /// <returns>IHostBuilder</returns>
    internal static IHostBuilder ConfigureWinFormsShell<TShell>(IHostBuilder hostBuilder)
        where TShell : Form, IWinFormsShell =>    
        ConfigureWinForms<TShell>(hostBuilder);    

    private static void InnerUseWinFormsLifetime(IDictionary<object, object> properties)
    {
        TryRetrieveWinFormsContext(properties, out var winFormsContext);
        winFormsContext.IsLifetimeLinked = true;
    }

    private static void InnerConfigureWinForms(IDictionary<object, object> properties, IServiceCollection serviceCollection, Action<IWinFormsContext> configureAction = null)
    {
        if (!TryRetrieveWinFormsContext(properties, out var winFormsContext))
        {
            serviceCollection
                .AddSingleton(winFormsContext)
                .AddSingleton(serviceProvider => new WinFormsThread(serviceProvider))
                .AddHostedService<WinFormsHostedService>();
        }
        configureAction?.Invoke(winFormsContext);
    }

    private static void InnerConfigureWinForms<TView>(IDictionary<object, object> properties, IServiceCollection serviceCollection, Action<IWinFormsContext> configureAction = null)
        where TView : Form
    {
        InnerConfigureWinForms(properties, serviceCollection, configureAction);

        serviceCollection.AddSingleton<TView>();

        // Check if it also implements IWinFormsShell so we can register it as this
        var viewType = typeof(TView);
        var shellInterfaceType = typeof(IWinFormsShell);
        if (shellInterfaceType.IsAssignableFrom(viewType))
        {
            serviceCollection.AddSingleton(shellInterfaceType, serviceProvider => serviceProvider.GetRequiredService<TView>());
        }
    }
}
