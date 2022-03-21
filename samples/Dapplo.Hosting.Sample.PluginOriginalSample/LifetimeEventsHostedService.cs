// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Dapplo.Hosting.Sample.FrameworkLib;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dapplo.Hosting.Sample.PluginOriginalSample;

/// <summary>
/// Example for a IHostedService which tracks live-time events
/// </summary>
internal class LifetimeEventsHostedService : IHostedService
{
    private readonly ILogger logger;
    private readonly IHostApplicationLifetime hostApplicationLifetime;

    /// <summary>
    /// DI-Constructor of this LifetimeEventsHostedService
    /// </summary>
    /// <param name="logger">ILogger</param>
    /// <param name="hostApplicationLifetime">IHostApplicationLifetime</param>
    public LifetimeEventsHostedService(ILogger<LifetimeEventsHostedService> logger, IHostApplicationLifetime hostApplicationLifetime)
    {
        this.logger = logger;
        this.hostApplicationLifetime = hostApplicationLifetime;
        SomeStaticExampleClass.RegisteredServices.Add(nameof(LifetimeEventsHostedService));
    }

    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.hostApplicationLifetime.ApplicationStarted.Register(OnStarted);
        this.hostApplicationLifetime.ApplicationStopping.Register(OnStopping);
        this.hostApplicationLifetime.ApplicationStopped.Register(OnStopped);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private void OnStarted()
    {
        this.logger.LogInformation("OnStarted has been called.");

        // Perform post-startup activities here
    }

    private void OnStopping()
    {
        this.logger.LogInformation("OnStopping has been called.");

        // Perform on-stopping activities here
    }

    private void OnStopped()
    {
        this.logger.LogInformation("OnStopped has been called.");

        // Perform post-stopped activities here
    }
}