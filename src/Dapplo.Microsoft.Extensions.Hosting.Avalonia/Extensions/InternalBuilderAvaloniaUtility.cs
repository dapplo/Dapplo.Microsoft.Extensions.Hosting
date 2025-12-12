// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Dapplo.Microsoft.Extensions.Hosting.Avalonia.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.Avalonia;

internal static class InternalBuilderAvaloniaUtility
{
    private const string AvaloniaContextKey = "AvaloniaContext";

    /// <summary>
    /// Defines that stopping the Avalonia application also stops the host (application)
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <param name="shutdownMode">ShutdownMode default is OnLastWindowClose</param>
    /// <returns>IHostBuilder</returns>
    internal static IHostBuilder UseAvaloniaLifetime(IHostBuilder hostBuilder, ShutdownMode shutdownMode = ShutdownMode.OnLastWindowClose) =>    
        hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) => InnerUseAvaloniaLifetime(hostBuilder.Properties, shutdownMode));
    

    /// <summary>
    /// Configure an Avalonia application 
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <param name="configureDelegate">Action to configure Avalonia</param>
    /// <returns></returns>
    internal static IHostBuilder ConfigureAvalonia(IHostBuilder hostBuilder, Action<IAvaloniaBuilder> configureDelegate = null) =>     
        hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) => InnerConfigureAvalonia(hostBuilder.Properties, serviceCollection, configureDelegate));

    /// <summary>
    /// Defines that stopping the Avalonia application also stops the host (application)
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <param name="shutdownMode">ShutdownMode default is OnLastWindowClose</param>
    /// <returns>IHostApplicationBuilder</returns>
    internal static IHostApplicationBuilder UseAvaloniaLifetime(IHostApplicationBuilder hostApplicationBuilder, ShutdownMode shutdownMode = ShutdownMode.OnLastWindowClose)
    {
        InnerUseAvaloniaLifetime(hostApplicationBuilder.Properties, shutdownMode);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// Configure an Avalonia application 
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <param name="configureDelegate">Action to configure Avalonia</param>
    /// <returns></returns>
    internal static IHostApplicationBuilder ConfigureAvalonia(IHostApplicationBuilder hostApplicationBuilder, Action<IAvaloniaBuilder> configureDelegate = null)
    {
        InnerConfigureAvalonia(hostApplicationBuilder.Properties, hostApplicationBuilder.Services, configureDelegate);

        return hostApplicationBuilder;
    }

    /// <summary>
    /// Helper method to retrieve the IAvaloniaContext
    /// </summary>
    /// <param name="properties">IDictionary</param>
    /// <param name="avaloniaContext">IAvaloniaContext out value</param>
    /// <returns>bool if there was already an IAvaloniaContext</returns>
    private static bool TryRetrieveAvaloniaContext(IDictionary<object, object> properties, out IAvaloniaContext avaloniaContext)
    {
        if (properties.TryGetValue(AvaloniaContextKey, out var avaloniaContextAsObject))
        {
            avaloniaContext = (IAvaloniaContext)avaloniaContextAsObject;
            return true;

        }
        avaloniaContext = new AvaloniaContext();
        properties[AvaloniaContextKey] = avaloniaContext;
        return false;
    }

    private static void InnerUseAvaloniaLifetime(IDictionary<object, object> properties, ShutdownMode shutdownMode)
    {
        if (!TryRetrieveAvaloniaContext(properties, out var avaloniaContext))
        {
            throw new NotSupportedException("Please configure Avalonia first!");
        }

        avaloniaContext.ShutdownMode = shutdownMode;
        avaloniaContext.IsLifetimeLinked = true;
    }

    private static void InnerConfigureAvalonia(IDictionary<object, object> properties, IServiceCollection serviceCollection, Action<IAvaloniaBuilder> configureDelegate = null)
    {
        var avaloniaBuilder = new AvaloniaBuilder();
        configureDelegate?.Invoke(avaloniaBuilder);

        if (!TryRetrieveAvaloniaContext(properties, out var avaloniaContext))
        {
            serviceCollection.AddSingleton(avaloniaContext);
            
            // Create the AppBuilder
            var appBuilder = AppBuilder.Configure(() => null);
            
            // Allow custom configuration of the AppBuilder
            avaloniaBuilder.ConfigureAppBuilderAction?.Invoke(appBuilder);
            
            // Use platform defaults if not configured
            if (appBuilder.Instance == null)
            {
                appBuilder = AppBuilder.Configure(() => avaloniaBuilder.Application ?? new Application())
                    .UsePlatformDetect()
                    .LogToTrace();
                    
                avaloniaBuilder.ConfigureAppBuilderAction?.Invoke(appBuilder);
            }
            
            serviceCollection.AddSingleton(serviceProvider => new AvaloniaThread(serviceProvider, appBuilder));
            serviceCollection.AddHostedService<AvaloniaHostedService>();
        }
        avaloniaBuilder.ConfigureContextAction?.Invoke(avaloniaContext);


        if (avaloniaBuilder.ApplicationType != null)
        {
            // Check if the registered application does inherit Avalonia.Application
            var baseApplicationType = typeof(Application);
            if (!baseApplicationType.IsAssignableFrom(avaloniaBuilder.ApplicationType))
            {
                throw new ArgumentException("The registered Application type must inherit Avalonia.Application", nameof(configureDelegate));
            }

            if (avaloniaBuilder.Application != null)
            {
                // Add existing Application
                serviceCollection.AddSingleton(avaloniaBuilder.ApplicationType, avaloniaBuilder.Application);
            }
            else
            {
                serviceCollection.AddSingleton(avaloniaBuilder.ApplicationType);
            }

            if (avaloniaBuilder.ApplicationType != baseApplicationType)
            {
                serviceCollection.AddSingleton(serviceProvider => (Application)serviceProvider.GetRequiredService(avaloniaBuilder.ApplicationType));
            }
        }

        if (avaloniaBuilder.WindowTypes.Count > 0)
        {
            foreach (var avaloniaWindowType in avaloniaBuilder.WindowTypes)
            {
                serviceCollection.AddSingleton(avaloniaWindowType);

                // Check if it also implements IAvaloniaShell so we can register it as this
                var shellInterfaceType = typeof(IAvaloniaShell);
                if (shellInterfaceType.IsAssignableFrom(avaloniaWindowType))
                {
                    serviceCollection.AddSingleton(shellInterfaceType, serviceProvider => serviceProvider.GetRequiredService(avaloniaWindowType));
                }
            }
        }
    }
}
