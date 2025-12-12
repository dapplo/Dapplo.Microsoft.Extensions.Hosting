#if USE_HOST_APPLICATION_BUILDER
// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.Microsoft.Extensions.Hosting.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Dapplo.Microsoft.Extensions.Hosting.AppServices;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;
using Microsoft.Extensions.DependencyInjection;

namespace Dapplo.Hosting.Sample.WpfDemo;

public static class Program
{
    private const string AppSettingsFilePrefix = "appsettings";
    private const string HostSettingsFile = "hostsettings.json";
    private const string Prefix = "PREFIX_";

    public static Task Main(string[] args)
    {
        var executableLocation = Path.GetDirectoryName(typeof(Program).Assembly.Location) ?? throw new NotSupportedException("Can't start without location.");
        var hostApplicationBuilderSettings = new HostApplicationBuilderSettings()
        {
            Args = args,
        };


        // Issue loading environment name from hostsettings.json file
        // For more details see https://github.com/dotnet/runtime/issues/97930 (Unable to configure host environment from a JSON settings file when using Host.CreateApplicationBuilder)
        var environmentName = GetEnvironmentNameFromHostSettingsFile();
        if (!string.IsNullOrEmpty(environmentName))
        {
            hostApplicationBuilderSettings.EnvironmentName = environmentName;
        }

        var builder = Host.CreateApplicationBuilder(hostApplicationBuilderSettings);
       
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();

        builder.ConfigureSingleInstance(builder =>
        {
            builder.MutexId = "{C3CC6C8F-B40C-4EC2-A540-1D4B8FFFB60D}";
            builder.WhenNotFirstInstance = (hostingEnvironment, logger) =>
            {
                // This is called when an instance was already started, this is in the second instance
                logger.LogWarning("Application {applicationName} already running.", hostingEnvironment.ApplicationName);
            };
        })
            .ConfigurePlugins(pluginBuilder =>
            {
                var runtime = Path.GetFileName(executableLocation);
                var parentDirectory = Directory.GetParent(executableLocation).FullName;
                var configuration = Path.GetFileName(parentDirectory);
                var basePath = Path.Combine(executableLocation, @"..\..\..\..\");
                // Specify the location from where the Dll's are "globbed"
                pluginBuilder.AddScanDirectories(basePath);
                // Add the framework libraries which can be found with the specified globs
                pluginBuilder.IncludeFrameworks(@$"**\bin\{configuration}\netstandard2.0\*.FrameworkLib.dll");
                // Add the plugins which can be found with the specified globs
                pluginBuilder.IncludePlugins(@$"**\bin\{configuration}\{runtime}\*.Sample.Plugin*.dll");
            })
            .ConfigureWpf(wpfBuilder =>
            {
                wpfBuilder.UseApplication<MyApplication>();
                wpfBuilder.UseWindow<MainWindow>();
            })
            .UseWpfLifetime()
            ;

        // Make OtherWindow available for DI to the MainWindow, but not as singleton
        builder.Services.AddTransient<OtherWindow>();

        var host = builder.Build();


        /*

        var host = new HostBuilder()
            .ConfigureLogging()
            .ConfigureConfiguration(args)
            .ConfigureSingleInstance(builder =>
            {
                builder.MutexId = "{C3CC6C8F-B40C-4EC2-A540-1D4B8FFFB60D}";
                builder.WhenNotFirstInstance = (hostingEnvironment, logger) =>
                {
                    // This is called when an instance was already started, this is in the second instance
                    logger.LogWarning("Application {applicationName} already running.", hostingEnvironment.ApplicationName);
                };
            })
            .ConfigurePlugins(pluginBuilder =>
            {
                var runtime = Path.GetFileName(executableLocation);
                var parentDirectory = Directory.GetParent(executableLocation).FullName;
                var configuration = Path.GetFileName(parentDirectory);
                var basePath = Path.Combine(executableLocation, @"..\..\..\..\");
                // Specify the location from where the Dll's are "globbed"
                pluginBuilder.AddScanDirectories(basePath);
                // Add the framework libraries which can be found with the specified globs
                pluginBuilder.IncludeFrameworks(@$"**\bin\{configuration}\netstandard2.0\*.FrameworkLib.dll");
                // Add the plugins which can be found with the specified globs
                pluginBuilder.IncludePlugins(@$"**\bin\{configuration}\{runtime}\*.Sample.Plugin*.dll");
            })
            .ConfigureServices(serviceCollection =>
                // Make OtherWindow available for DI to the MainWindow, but not as singleton
                serviceCollection.AddTransient<OtherWindow>())
            .ConfigureWpf(wpfBuilder =>
            {
                wpfBuilder.UseApplication<MyApplication>();
                wpfBuilder.UseWindow<MainWindow>();
            })
            .UseWpfLifetime()
            .UseConsoleLifetime()
            .Build();
        */

        Console.WriteLine("Run!");

        return host.RunAsync();
    }

    private static string GetEnvironmentNameFromHostSettingsFile()
    {
        string environmentName = null;

        var hostSettingsFilePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), HostSettingsFile);

        if (System.IO.File.Exists(hostSettingsFilePath))
        {
            var jsonData = File.ReadAllText(hostSettingsFilePath);
            var jsonHostSettingsObject = System.Text.Json.Nodes.JsonObject.Parse(jsonData);

            if (jsonHostSettingsObject != null)
            {
                var environmentNode = jsonHostSettingsObject["environment"];
                if (environmentNode != null)
                {
                    environmentName = environmentNode.GetValue<string>();
                }
            }
        }

        return environmentName;
    }
}

#endif
