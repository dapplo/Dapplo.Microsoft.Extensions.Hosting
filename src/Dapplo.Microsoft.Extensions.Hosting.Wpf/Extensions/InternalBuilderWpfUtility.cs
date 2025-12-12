using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Dapplo.Microsoft.Extensions.Hosting.Wpf.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.Wpf;

internal static class InternalBuilderWpfUtility
{
    private const string WpfContextKey = "WpfContext";

    /// <summary>
    /// Defines that stopping the WPF application also stops the host (application)
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <param name="shutdownMode">ShutdownMode default is OnLastWindowClose</param>
    /// <returns>IHostBuilder</returns>
    internal static IHostBuilder UseWpfLifetime(IHostBuilder hostBuilder, ShutdownMode shutdownMode = ShutdownMode.OnLastWindowClose) =>    
        hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) => InnerUseWpfLifetime(hostBuilder.Properties, shutdownMode));
    

    /// <summary>
    /// Configure an WPF application 
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <param name="configureDelegate">Action to configure Wpf</param>
    /// <returns></returns>
    internal static IHostBuilder ConfigureWpf(IHostBuilder hostBuilder, Action<IWpfBuilder> configureDelegate = null) =>     
        hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) => InnerConfigureWpf(hostBuilder.Properties, serviceCollection, configureDelegate));

    /// <summary>
    /// Defines that stopping the WPF application also stops the host (application)
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <param name="shutdownMode">ShutdownMode default is OnLastWindowClose</param>
    /// <returns>IHostApplicationBuilder</returns>
    internal static IHostApplicationBuilder UseWpfLifetime(IHostApplicationBuilder hostApplicationBuilder, ShutdownMode shutdownMode = ShutdownMode.OnLastWindowClose)
    {
        InnerUseWpfLifetime(hostApplicationBuilder.Properties, shutdownMode);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// Configure an WPF application 
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <param name="configureDelegate">Action to configure Wpf</param>
    /// <returns></returns>
    internal static IHostApplicationBuilder ConfigureWpf(IHostApplicationBuilder hostApplicationBuilder, Action<IWpfBuilder> configureDelegate = null)
    {
        InnerConfigureWpf(hostApplicationBuilder.Properties, hostApplicationBuilder.Services, configureDelegate);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// Helper method to retrieve the IWpfContext
    /// </summary>
    /// <param name="properties">IDictionary</param>
    /// <param name="wpfContext">IWpfContext out value</param>
    /// <returns>bool if there was already an IWpfContext</returns>
    private static bool TryRetrieveWpfContext(IDictionary<object, object> properties, out IWpfContext wpfContext)
    {
        if (properties.TryGetValue(WpfContextKey, out var wpfContextAsObject))
        {
            wpfContext = (IWpfContext)wpfContextAsObject;
            return true;

        }
        wpfContext = new WpfContext();
        properties[WpfContextKey] = wpfContext;
        return false;
    }

    private static void InnerUseWpfLifetime(IDictionary<object, object> properties, ShutdownMode shutdownMode)
    {
        if (!TryRetrieveWpfContext(properties, out var wpfContext))
        {
            throw new NotSupportedException("Please configure WPF first!");
        }

        wpfContext.ShutdownMode = shutdownMode;
        wpfContext.IsLifetimeLinked = true;
    }

    private static void InnerConfigureWpf(IDictionary<object, object> properties, IServiceCollection serviceCollection, Action<IWpfBuilder> configureDelegate = null)
    {
        var wpfBuilder = new WpfBuilder();
        configureDelegate?.Invoke(wpfBuilder);

        if (!TryRetrieveWpfContext(properties, out var wpfContext))
        {
            serviceCollection.AddSingleton(wpfContext);
            serviceCollection.AddSingleton(serviceProvider => new WpfThread(serviceProvider));
            serviceCollection.AddHostedService<WpfHostedService>();
        }
        wpfBuilder.ConfigureContextAction?.Invoke(wpfContext);


        if (wpfBuilder.ApplicationType != null)
        {
            // Check if the registered application does inherit System.Windows.Application
            var baseApplicationType = typeof(Application);
            if (!baseApplicationType.IsAssignableFrom(wpfBuilder.ApplicationType))
            {
                throw new ArgumentException("The registered Application type inherit System.Windows.Application", nameof(configureDelegate));
            }

            if (wpfBuilder.Application != null)
            {
                // Add existing Application
                serviceCollection.AddSingleton(wpfBuilder.ApplicationType, wpfBuilder.Application);
            }
            else
            {
                serviceCollection.AddSingleton(wpfBuilder.ApplicationType);
            }

            if (wpfBuilder.ApplicationType != baseApplicationType)
            {
                serviceCollection.AddSingleton(serviceProvider => (Application)serviceProvider.GetRequiredService(wpfBuilder.ApplicationType));
            }
        }

        if (wpfBuilder.WindowTypes.Count > 0)
        {
            foreach (var wpfWindowType in wpfBuilder.WindowTypes)
            {
                serviceCollection.AddSingleton(wpfWindowType);

                // Check if it also implements IWpfShell so we can register it as this
                var shellInterfaceType = typeof(IWpfShell);
                if (shellInterfaceType.IsAssignableFrom(wpfWindowType))
                {
                    serviceCollection.AddSingleton(shellInterfaceType, serviceProvider => serviceProvider.GetRequiredService(wpfWindowType));
                }
            }
        }
    }
}
