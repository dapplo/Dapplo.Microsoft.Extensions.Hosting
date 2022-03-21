// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Caliburn.Micro;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Dapplo.Microsoft.Extensions.Hosting.CaliburnMicro;

namespace Dapplo.Hosting.Sample.CaliburnMicroDemo.ViewModels;

/// <summary>
/// Just a simple main view model
/// </summary>
public class MainViewModel : Screen, ICaliburnMicroShell
{
    private readonly IWpfContext wpfContext;
    private readonly IServiceProvider serviceProvider;
    private readonly IWindowManager windowManager;

    public MainViewModel(IWpfContext wpfContext, IServiceProvider serviceProvider, IWindowManager windowManager)
    {
        this.wpfContext = wpfContext;
        this.serviceProvider = serviceProvider;
        this.windowManager = windowManager;
    }

    public Task Open()
    {
        var otherWindow = this.serviceProvider.GetService<OtherViewModel>();
        return this.windowManager.ShowWindowAsync(otherWindow);
    }

    public void Exit()
    {
        this.wpfContext.WpfApplication.Shutdown();
    }
}