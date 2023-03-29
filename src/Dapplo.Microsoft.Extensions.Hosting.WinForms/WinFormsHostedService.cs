// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapplo.Microsoft.Extensions.Hosting.WinForms.Internals;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dapplo.Microsoft.Extensions.Hosting.WinForms;

/// <summary>
/// This hosts a WinForms service, making sure the lifecycle is managed
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class WinFormsHostedService : IHostedService
{
    private readonly ILogger<WinFormsHostedService> logger;
    private readonly WinFormsThread winFormsThread;
    private readonly IWinFormsContext winFormsContext;

    /// <summary>
    /// The constructor which takes all the DI objects
    /// </summary>
    /// <param name="logger">ILogger</param>
    /// <param name="winFormsThread">WinFormsThread</param>
    /// <param name="winFormsContext">IWinFormsContext</param>
    public WinFormsHostedService(ILogger<WinFormsHostedService> logger, WinFormsThread winFormsThread, IWinFormsContext winFormsContext)
    {
        this.logger = logger;
        this.winFormsThread = winFormsThread;
        this.winFormsContext = winFormsContext;
    }

    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.CompletedTask;
        }
        this.winFormsThread.Start();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (this.winFormsContext.IsRunning)
        {
            this.logger.LogDebug("Stopping WinForms application.");
            await this.winFormsContext.Dispatcher.InvokeAsync(()=>
            {
                // Graceful close, otherwise finalizes try to dispose forms.
                foreach (var form in Application.OpenForms.Cast<Form>().ToList())
                {
                    try
                    {
                        form.Close();
                        form.Dispose();
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogWarning(ex, "Couldn't cleanup a Form");
                    }
                }
                Application.ExitThread();
            });
        }
    }
}