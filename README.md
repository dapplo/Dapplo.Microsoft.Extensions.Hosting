# Dapplo.Microsoft.Extensions.Hosting

[![Build Status](https://dev.azure.com/Dapplo/Dapplo.Microsoft.Extensions.Hosting/_apis/build/status/dapplo.Dapplo.Microsoft.Extensions.Hosting?branchName=master)](https://dev.azure.com/Dapplo/Dapplo.Microsoft.Extensions.Hosting/_build/latest?definitionId=6&branchName=master)

This repository contains extensions which work with the Microsoft.Extensions.Hosting 


Dapplo.Microsoft.Extensions.Hosting.Plugins
--------------------------------------------
Extend generic host based application with plugins

This extension adds plug-in support to generic host based application.

You can simply add the location of plug-ins by specifying globs to find your plug-in assemblies.
This can be both files to include and / or exclude.
Each located plug-ins is loaded into it's own AssemblyLoadContext, dependencies are found and loaded in the same AssemblyLoadContext via an AssemblyDependencyResolver (which was introduced in dotnet core 3.0).

Here is an example how to use the loading:
```
    public static async Task Main(string[] args)
    {
        var host = new HostBuilder()
            // Specify the location from where the dll's are "globbed"
            .UseContentRoot(@"..\..\..\..\")
            // Add the plugins which can be found with the specified globs
            .AddPlugins(@"**\bin\**\*.Plugin*.dll")
            // Files can be excluded
            .AddPlugins(matcher => matcher.AddExclude("*Demo*"))
            .UseConsoleLifetime()
            .Build();

        Console.WriteLine("Run!");
        await host.RunAsync();
    }
```

The DLL which is your plugin should have at least one class which implements IPlugin, this implementation can configure the HostBuilderContext.
```
    /// <summary>
    /// This plug-in configures the HostBuilderContext to have the hosted services from the online example
    /// </summary>
    public class Plugin : IPlugin
    {
        /// <inheritdoc />
        public void ConfigureHost(HostBuilderContext hostBuilderContext, IServiceCollection serviceCollection)
        {
            serviceCollection.AddHostedService<LifetimeEventsHostedService>();
            serviceCollection.AddHostedService<TimedHostedService>();
        }
    }
```
