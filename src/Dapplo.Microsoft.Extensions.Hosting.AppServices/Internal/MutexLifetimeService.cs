// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dapplo.Microsoft.Extensions.Hosting.AppServices.Internal;

/// <summary>
/// This maintains the mutex lifetime
/// </summary>
internal class MutexLifetimeService : IHostedService
{
    private readonly ILogger logger;
    private readonly IHostEnvironment hostEnvironment;
    private readonly IHostApplicationLifetime hostApplicationLifetime;
    private readonly IMutexBuilder mutexBuilder;
    private ResourceMutex resourceMutex;

    public MutexLifetimeService(ILogger<MutexLifetimeService> logger, IHostEnvironment hostEnvironment, IHostApplicationLifetime hostApplicationLifetime, IMutexBuilder mutexBuilder)
    {
        this.logger = logger;
        this.hostEnvironment = hostEnvironment;
        this.hostApplicationLifetime = hostApplicationLifetime;
        this.mutexBuilder = mutexBuilder;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.resourceMutex = ResourceMutex.Create(null, this.mutexBuilder.MutexId, this.hostEnvironment.ApplicationName, this.mutexBuilder.IsGlobal);

        this.hostApplicationLifetime.ApplicationStopping.Register(OnStopping);
        if (!this.resourceMutex.IsLocked)
        {
            this.mutexBuilder.WhenNotFirstInstance?.Invoke(this.hostEnvironment, this.logger);
            this.logger.LogDebug("Application {applicationName} already running, stopping application.", this.hostEnvironment.ApplicationName);
            this.hostApplicationLifetime.StopApplication();
        }

        return Task.CompletedTask;
    }

    private void OnStopping()
    {
        this.logger.LogInformation("OnStopping has been called, closing mutex.");
        this.resourceMutex.Dispose();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
