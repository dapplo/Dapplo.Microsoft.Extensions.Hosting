using Microsoft.Extensions.Logging;
using System.Windows;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;

namespace Dapplo.Hosting.Sample.WpfDemo
{
    /// <summary>
    /// A simple example WPF window
    /// </summary>
    public partial class MainWindow : IShell
    {
        private readonly ILogger<MainWindow> _logger;

        public MainWindow(ILogger<MainWindow> logger)
        {
            InitializeComponent();
            _logger = logger;
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            _logger.LogInformation("Exit-Button was clicked!");
            Application.Current.Shutdown();
        }
    }
}
