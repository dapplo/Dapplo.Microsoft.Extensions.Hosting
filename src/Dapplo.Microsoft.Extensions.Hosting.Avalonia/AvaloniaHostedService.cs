// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Dapplo.Microsoft.Extensions.Hosting.Avalonia.Internals;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dapplo.Microsoft.Extensions.Hosting.Avalonia;

/// <summary>
/// This hosts an Avalonia service, making sure the lifecycle is managed
/// </summary>
public class AvaloniaHostedService : IHostedService
{
    private readonly ILogger<AvaloniaHostedService> logger;
    private readonly AvaloniaThread avaloniaThread;
    private readonly IAvaloniaContext avaloniaContext;

    /// <summary>
    /// The constructor which takes all the DI objects
    /// </summary>
    /// <param name="logger">ILogger</param>
    /// <param name="avaloniaThread">AvaloniaThread</param>
    /// <param name="avaloniaContext">IAvaloniaContext</param>
    public AvaloniaHostedService(ILogger<AvaloniaHostedService> logger, AvaloniaThread avaloniaThread, IAvaloniaContext avaloniaContext)
    {
        this.logger = logger;
        this.avaloniaThread = avaloniaThread;
        this.avaloniaContext = avaloniaContext;
    }

    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.CompletedTask;
        }
            
        // Make the UI thread go
        this.avaloniaThread.Start();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (this.avaloniaContext.IsRunning)
        {
            this.logger.LogDebug("Stopping Avalonia due to application exit.");
            // Stop application
            await this.avaloniaContext.Dispatcher.InvokeAsync(() => this.avaloniaContext.ApplicationLifetime?.Shutdown());
        }
    }
}
