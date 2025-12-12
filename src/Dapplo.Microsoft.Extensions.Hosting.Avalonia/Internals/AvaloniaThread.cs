// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Dapplo.Microsoft.Extensions.Hosting.UiThread;
using Microsoft.Extensions.DependencyInjection;

namespace Dapplo.Microsoft.Extensions.Hosting.Avalonia.Internals;

/// <summary>
/// This contains the logic for the Avalonia thread
/// </summary>
public class AvaloniaThread : BaseUiThread<IAvaloniaContext>
{
    private readonly AppBuilder appBuilder;

    /// <summary>
    /// This will create the AvaloniaThread
    /// </summary>
    /// <param name="serviceProvider">IServiceProvider</param>
    /// <param name="appBuilder">AppBuilder</param>
    public AvaloniaThread(IServiceProvider serviceProvider, AppBuilder appBuilder) : base(serviceProvider)
    {
        this.appBuilder = appBuilder;
    }

    /// <inheritdoc />
    protected override void PreUiThreadStart()
    {
        // Initialize Avalonia
        var application = ServiceProvider.GetService<Application>();
        if (application != null)
        {
            UiContext.AvaloniaApplication = application;
        }
        else
        {
            // Build the Avalonia application
            appBuilder.SetupWithoutStarting();
            UiContext.AvaloniaApplication = Application.Current;
        }
    }

    /// <inheritdoc />
    protected override void UiThreadStart()
    {
        var lifetime = new ClassicDesktopStyleApplicationLifetime
        {
            ShutdownMode = UiContext.ShutdownMode
        };

        UiContext.ApplicationLifetime = lifetime;
        UiContext.AvaloniaApplication.ApplicationLifetime = lifetime;

        // Register to the Avalonia application exit to stop the host application
        lifetime.Exit += (s, e) => HandleApplicationExit();

        // Mark the application as running
        UiContext.IsRunning = true;

        // Use the provided IAvaloniaService
        var avaloniaServices = ServiceProvider.GetServices<IAvaloniaService>();
        foreach (var avaloniaService in avaloniaServices)
        {
            avaloniaService.Initialize(UiContext.AvaloniaApplication);
        }

        // Run the Avalonia application in this thread which was specifically created for it, with the specified shell
        var shellWindows = ServiceProvider.GetServices<IAvaloniaShell>().Cast<Window>().ToList();

        switch (shellWindows.Count)
        {
            case 1:
                lifetime.MainWindow = shellWindows[0];
                break;
            case 0:
                // No shell windows, we'll let the application handle it
                break;
            default:
                // Multiple windows, use the first as main window
                lifetime.MainWindow = shellWindows[0];
                // Show additional windows after startup
                lifetime.Startup += (sender, args) =>
                {
                    for (int i = 1; i < shellWindows.Count; i++)
                    {
                        shellWindows[i]?.Show();
                    }
                };
                break;
        }

        // Start the Avalonia lifetime
        lifetime.Start(Array.Empty<string>());
    }
}
