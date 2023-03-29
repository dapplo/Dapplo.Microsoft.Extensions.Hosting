// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using System.Windows;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;
using Microsoft.Extensions.DependencyInjection;

namespace Dapplo.Hosting.Sample.WpfDemo;

/// <summary>
/// A simple example WPF window
/// </summary>
public partial class MainWindow : IWpfShell
{
    private readonly ILogger<MainWindow> logger;
    private readonly IServiceProvider serviceProvider;

    public MainWindow(ILogger<MainWindow> logger, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        this.logger = logger;
        this.serviceProvider = serviceProvider;
    }

    private void ButtonExit_Click(object sender, RoutedEventArgs e)
    {
        this.logger.LogInformation("Exit-Button was clicked!");
        Application.Current.Shutdown();
    }
        
    private void ButtonAnotherWindow_Click(object sender, RoutedEventArgs e)
    {
        var otherWindow = this.serviceProvider.GetService<OtherWindow>();
        otherWindow.Show();
    }
        
}