using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;
using Dapplo.Microsoft.Extensions.Hosting.CaliburnMicro;
using Microsoft.Extensions.DependencyInjection;

namespace Dapplo.Microsoft.Extensions.Hosting.CaliburnMicro.Template.ViewModels
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

        public Task Open()
        {
            var otherWindow = _serviceProvider.GetService<OtherViewModel>();
            return _windowManager.ShowWindowAsync(otherWindow);
        }

        public void Exit()
        {
            _wpfContext.WpfApplication.Shutdown();
        }
    }
}
