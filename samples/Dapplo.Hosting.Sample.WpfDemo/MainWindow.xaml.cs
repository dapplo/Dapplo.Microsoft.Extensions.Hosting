using Microsoft.Extensions.Logging;
using System.Windows;

namespace Dapplo.Hosting.Sample.WpfDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IShell
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
