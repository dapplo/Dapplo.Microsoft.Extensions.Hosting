// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Caliburn.Micro;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;
using System;
using Microsoft.Extensions.DependencyInjection;
using Dapplo.Microsoft.Extensions.Hosting.CaliburnMicro;

namespace Dapplo.Hosting.Sample.CaliburnMicroDemo.ViewModels
{
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
