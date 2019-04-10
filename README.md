# Dapplo.Microsoft.Extensions.Hosting

[![Build Status](https://dev.azure.com/Dapplo/Dapplo.Microsoft.Extensions.Hosting/_apis/build/status/dapplo.Dapplo.Microsoft.Extensions.Hosting?branchName=master)](https://dev.azure.com/Dapplo/Dapplo.Microsoft.Extensions.Hosting/_build/latest?definitionId=6&branchName=master)

This repository contains extensions for Microsoft.Extensions.Hosting

Dapplo.Microsoft.Extensions.Hosting.Plugins
--------------------------------------------
[![Nuget](https://img.shields.io/nuget/v/Dapplo.Microsoft.Extensions.Hosting.Plugins.svg)](https://www.nuget.org/packages/Dapplo.Microsoft.Extensions.Hosting.Plugins/)

This extension adds plug-in support to generic host based dotnet core 3.0 applications.

You can simply add the location of plug-ins by specifying globs to find your plug-in assemblies.
This can be both files to include and / or exclude.
Each located plug-ins is loaded into it's own AssemblyLoadContext, dependencies are found and loaded in the same AssemblyLoadContext via an AssemblyDependencyResolver (which was introduced in dotnet core 3.0).

[Here](https://github.com/dapplo/Dapplo.Microsoft.Extensions.Hosting/blob/master/samples/Dapplo.Hosting.Sample.ConsoleDemo/Program.cs#L27)
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


Dapplo.Microsoft.Extensions.Hosting.AppServices
-----------------------------------------------
[![Nuget](https://img.shields.io/nuget/v/Dapplo.Microsoft.Extensions.Hosting.AppServices.svg)](https://www.nuget.org/packages/Dapplo.Microsoft.Extensions.Hosting.AppServices/)

This extension adds some application services for Win32 applications, currently only the mutex functionality is included.

[Here](https://github.com/dapplo/Dapplo.Microsoft.Extensions.Hosting/blob/master/samples/Dapplo.Hosting.Sample.WinFormsDemo/Program.cs#L25) is an example how to make sure your application only runs once.

In general you call hostBuilder.ConfigureSingleInstance and supply a mutex id.



Dapplo.Microsoft.Extensions.Hosting.WinForms
--------------------------------------------

[![Nuget](https://img.shields.io/nuget/v/Dapplo.Microsoft.Extensions.Hosting.WinForms.svg)](https://www.nuget.org/packages/Dapplo.Microsoft.Extensions.Hosting.WinForms/)

This extension adds WinForms support to generic host based dotnet core 3.0 applications.
With this you can enhance your application with a UI, and use all the services provided by the generic host like DI, logging etc.

[Here](https://github.com/dapplo/Dapplo.Microsoft.Extensions.Hosting/blob/master/samples/Dapplo.Hosting.Sample.WinFormsDemo/Program.cs#L48) is an example how to start your application with a Form1 and have the application automatically shutdown whenever you exit the Form1. To make this possible Form1 must implement a marker interface, which currently has no methods, called IWinFormsShell. The IWinFormsShell is considered the main entry point of your UI. You only specify the type, the instance will be created at a later time by the generic host and will automatically go through the DI process.

This means you can have a constructor which requests a logger, or other forms.


Dapplo.Microsoft.Extensions.Hosting.Wpf
---------------------------------------

[![Nuget](https://img.shields.io/nuget/v/Dapplo.Microsoft.Extensions.Hosting.Wpf.svg)](https://www.nuget.org/packages/Dapplo.Microsoft.Extensions.Hosting.Wpf/)

This extension adds WPF support to generic host based dotnet core 3.0 applications.
With this you can enhance your application with a UI, and use all the services provided by the generic host like DI, logging etc.

[Here](https://github.com/dapplo/Dapplo.Microsoft.Extensions.Hosting/blob/master/samples/Dapplo.Hosting.Sample.WpfDemo/Program.cs#L48) is an example how to start your application with a MainWindow and have the application automatically shutdown whenever you exit the MainWindow. To make this possible MainWindow must implement a marker interface, which currently has no methods, called IWpfShell. The IWpfShell is considered the main entry point of your UI. You only specify the type, the instance will be created at a later time by the generic host and will automatically go through the DI process.

This means you can have a constructor which requests a logger, or other windows.
