using System;
using Caliburn.Micro;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;
using Dapplo.Microsoft.Extensions.Hosting.CaliburnMicro;
using Microsoft.Extensions.DependencyInjection;

namespace Dapplo.Hosting.CaliburnMicroTemplate.ViewModels
{
    /// <summary>
    /// A simple main view model
    /// </summary>
    public class MainViewModel : Screen, ICaliburnMicroShell
    {
        private readonly IWpfContext _wpfContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly IWindowManager _windowManager;

        public MainViewModel(IWpfContext wpfContext, IServiceProvider serviceProvider, IWindowManager windowManager)
        {
            _wpfContext = wpfContext;
            _serviceProvider = serviceProvider;
            _windowManager = windowManager;
        }

        public void Open()
        {
            var otherWindow = _serviceProvider.GetService<OtherViewModel>();
            _windowManager.ShowWindow(otherWindow);
        }

        public void Exit()
        {
            _wpfContext.WpfApplication.Shutdown();
        }
    }
}
