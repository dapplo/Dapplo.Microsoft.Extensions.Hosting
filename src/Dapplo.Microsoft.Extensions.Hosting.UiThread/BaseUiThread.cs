// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.UiThread;

/// <summary>
/// This contains the base logic for the UI thread (WPF, WinForms)
/// </summary>
public abstract class BaseUiThread<T> where T : class, IUiContext
{
    private readonly ManualResetEvent serviceManualResetEvent = new(false);
    private readonly IHostApplicationLifetime hostApplicationLifetime;

    /// <summary>
    /// The IUiContext
    /// </summary>
    protected T UiContext { get; }

    /// <summary>
    /// The IServiceProvider used by all IUiContext implementations
    /// </summary>
    protected IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Constructor which is called from the IWinFormsContext
    /// </summary>
    /// <param name="serviceProvider">IServiceProvider this is scoped, this disposed </param>
    protected BaseUiThread(IServiceProvider serviceProvider)
    {
        UiContext = serviceProvider.GetService<T>();
        this.hostApplicationLifetime = serviceProvider.GetService<IHostApplicationLifetime>();
        ServiceProvider = serviceProvider;
        // Create a thread which runs the UI
        var newUiThread = new Thread(InternalUiThreadStart)
        {
            IsBackground = true
        };
        // Set the apartment state
        newUiThread.SetApartmentState(ApartmentState.STA);
        // Start the new UI thread
        newUiThread.Start();
    }

    /// <summary>
    /// Start the DI service on the thread
    /// </summary>
    public void Start()
    {
        // Make the UI thread go
        this.serviceManualResetEvent.Set();
    }

    /// <summary>
    /// Start UI
    /// </summary>
    private void InternalUiThreadStart()
    {
        // Do the pre initialization, if any
        PreUiThreadStart();
        // Wait for the startup
        this.serviceManualResetEvent.WaitOne();
        // Run the application
        UiContext.IsRunning = true;
        // Run the actual code
        UiThreadStart();
    }

    /// <summary>
    /// Do all the pre work, before the UI thread can start
    /// </summary>
    protected abstract void PreUiThreadStart();

    /// <summary>
    /// Implement all the code which is needed to run the actual UI
    /// </summary>
    protected abstract void UiThreadStart();

    /// <summary>
    /// Handle the application exit
    /// </summary>
    protected void HandleApplicationExit()
    {
        UiContext.IsRunning = false;
        if (!UiContext.IsLifetimeLinked)
        {
            return;
        }

        //_logger.LogDebug("Stopping host application due to WinForms application exit.");
        if (this.hostApplicationLifetime.ApplicationStopped.IsCancellationRequested || this.hostApplicationLifetime.ApplicationStopping.IsCancellationRequested)
        {
            return;
        }
        this.hostApplicationLifetime.StopApplication();
    }
}
