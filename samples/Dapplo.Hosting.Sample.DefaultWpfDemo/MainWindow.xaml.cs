using System;
using System.Windows;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dapplo.Hosting.Sample.DefaultWpfDemo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IWpfShell
{
    private readonly ILogger<MainWindow>? logger;
    private readonly IServiceProvider? serviceProvider;

    public MainWindow(ILogger<MainWindow> logger, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        this.logger = logger;
        this.serviceProvider = serviceProvider;
    }

    public MainWindow()
    {
        // if App.xaml StartupUri is configured to MainWindow, this constructor is called
        InitializeComponent();
    }

    private void ButtonExit_Click(object sender, RoutedEventArgs e)
    {
        this.logger?.LogInformation("Exit-Button was clicked!");
        Application.Current.Shutdown();
    }

    private void ButtonAnotherWindow_Click(object sender, RoutedEventArgs e)
    {
        var otherWindow = this.serviceProvider?.GetService<OtherWindow>();
        otherWindow?.Show();
    }
}
