// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Dapplo.Microsoft.Extensions.Hosting.Avalonia;
using Microsoft.Extensions.Logging;

namespace Dapplo.Microsoft.Extensions.Hosting.Avalonia.Template;

/// <summary>
/// Main application window
/// </summary>
public partial class MainWindow : Window, IAvaloniaShell
{
    private readonly ILogger<MainWindow> logger;

    public MainWindow(ILogger<MainWindow> logger)
    {
        InitializeComponent();
        this.logger = logger;
        
        // Attach event handlers
        var exitButton = this.FindControl<Button>("ExitButton");
        if (exitButton != null)
        {
            exitButton.Click += ExitButton_Click;
        }
    }

    private void ExitButton_Click(object? sender, RoutedEventArgs e)
    {
        this.logger.LogInformation("Exit-Button was clicked!");
        
        // Close the application
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            lifetime.Shutdown();
        }
    }
}
