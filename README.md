# Dapplo.Microsoft.Extensions.Hosting

[![Build Status](https://dev.azure.com/Dapplo/Dapplo.Microsoft.Extensions.Hosting/_apis/build/status/dapplo.Dapplo.Microsoft.Extensions.Hosting?branchName=master)](https://dev.azure.com/Dapplo/Dapplo.Microsoft.Extensions.Hosting/_build/latest?definitionId=6&branchName=master)

This repository contains extensions for Microsoft.Extensions.Hosting


Dapplo.Microsoft.Extensions.Hosting.Plugins
--------------------------------------------
[![Nuget](https://img.shields.io/nuget/v/Dapplo.Microsoft.Extensions.Hosting.Plugins.svg)](https://www.nuget.org/packages/Dapplo.Microsoft.Extensions.Hosting.Plugins/)

This extension adds plug-in support to generic host based dotnet core 3.0 application.

You can simply add the location of plug-ins by specifying globs to find your plug-in assemblies.
This can be both files to include and / or exclude.
Each located plug-ins is loaded into it's own AssemblyLoadContext, dependencies are found and loaded in the same AssemblyLoadContext via an AssemblyDependencyResolver (which was introduced in dotnet core 3.0).

[Here](https://github.com/dapplo/Dapplo.Microsoft.Extensions.Hosting/blob/master/samples/Dapplo.Microsoft.Extensions.Hosting.ConsoleDemo/Program.cs#L27)
 is an example how to use the loading, and also how to handle framework assemblies.
 
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
