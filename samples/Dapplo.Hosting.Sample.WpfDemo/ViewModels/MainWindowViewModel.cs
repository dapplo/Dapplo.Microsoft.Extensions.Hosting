using System;
using System.Windows.Input;
using Dapplo.Hosting.Sample.WpfDemo.Commands;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;
using Microsoft.Extensions.Logging;

namespace Dapplo.Hosting.Sample.WpfDemo.ViewModels {
    /// <summary>
    /// A simple view model
    /// </summary>
    public class MainWindowViewModel : IWpfViewModel
    {
        private readonly ILogger<MainWindowViewModel> _logger;
        private readonly IServiceProvider _serviceProvider;

        public MainWindowViewModel(
          ILogger<MainWindowViewModel> logger,
          IServiceProvider serviceProvider,
          OpenOtherWindowCommand openOtherWindowCommand
          )
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            Message = "Hello .NET Core!";
            OpenOtherWindowCommand = openOtherWindowCommand;
        }

        public string Message { get; }

        public ICommand OpenOtherWindowCommand { get; }
    }
}
