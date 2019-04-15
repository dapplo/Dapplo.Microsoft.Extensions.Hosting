using System;
using System.IO;
using System.Threading.Tasks;
using Dapplo.Microsoft.Extensions.Hosting.AppServices;
using Dapplo.Microsoft.Extensions.Hosting.Plugins;
using Dapplo.Microsoft.Extensions.Hosting.WinForms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dapplo.Hosting.Sample.WinFormsDemo
{
    public static class Program
    {
        private const string AppSettingsFilePrefix = "appsettings";
        private const string HostSettingsFile = "hostsettings.json";
        private const string Prefix = "PREFIX_";

        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureLogging()
                .ConfigureConfiguration(args)
                .ConfigureSingleInstance(builder =>
                {
                    builder.MutexId = "{B9CE32C0-59AE-4AF0-BE39-5329AAFF4BE8}";
                    builder.WhenNotFirstInstance = (hostingEnvironment, logger) =>
                    {
                        // This is called when an instance was already started, this is in the second instance
                        logger.LogWarning("Application {0} already running.", hostingEnvironment.ApplicationName);
                    };
                })
                .ConfigurePlugins(pluginBuilder =>
                {
                    // Specify the location from where the Dll's are "globbed"
                    pluginBuilder.AddScanDirectories(Path.Combine(Directory.GetCurrentDirectory(), @"..\.."));
                    // Add the framework libraries which can be found with the specified globs
                    pluginBuilder.IncludeFrameworks(@"**\bin\**\*.FrameworkLib.dll");
                    // Add the plugins which can be found with the specified globs
                    pluginBuilder.IncludePlugins(@"**\bin\**\*.Plugin*.dll");
                })
                .ConfigureServices(serviceCollection =>
                {
                    // Make Form2 available for DI to Form1
                    serviceCollection.AddTransient<Form2>();
                })
                .ConfigureWinForms<Form1>()
                .UseWinFormsLifetime()
                .UseConsoleLifetime()
                .Build();

            Console.WriteLine(@"Run!");

            await host.RunAsync();
        }

        /// <summary>
        /// Configure the loggers
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <returns>IHostBuilder</returns>
        private static IHostBuilder ConfigureLogging(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureLogging((hostContext, configLogging) =>
            {
                configLogging.AddConsole();
                configLogging.AddDebug();
            });
        }

        /// <summary>
        /// Configure the configuration
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IHostBuilder ConfigureConfiguration(this IHostBuilder hostBuilder, string[] args)
        {
            return hostBuilder.ConfigureHostConfiguration(configHost =>
            {
                configHost.SetBasePath(Directory.GetCurrentDirectory());
                configHost.AddJsonFile(HostSettingsFile, optional: true);
                configHost.AddEnvironmentVariables(prefix: Prefix);
                configHost.AddCommandLine(args);
            })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddJsonFile(AppSettingsFilePrefix + ".json", optional: true);
                    if (!string.IsNullOrEmpty(hostContext.HostingEnvironment.EnvironmentName))
                    {
                        configApp.AddJsonFile(AppSettingsFilePrefix + $".{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    }
                    configApp.AddEnvironmentVariables(prefix: Prefix);
                    configApp.AddCommandLine(args);
                });
        }
    }
}
