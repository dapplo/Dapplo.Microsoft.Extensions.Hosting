// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Dapplo.Hosting.Sample.FrameworkLib;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dapplo.Hosting.Sample.PluginOriginalSample;

public class TimedHostedService : IHostedService, IDisposable
{
    private readonly ILogger logger;
    private Timer timer;

    public TimedHostedService(ILogger<TimedHostedService> logger)
    {
        this.logger = logger;
        SomeStaticExampleClass.RegisteredServices.Add(nameof(TimedHostedService));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Timed Background Service is starting.");

        this.timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        this.logger.LogInformation("Timed Background Service is working.");
        this.logger.LogInformation("Known registered Services {0}", string.Join(", ", SomeStaticExampleClass.RegisteredServices));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Timed Background Service is stopping.");

        this.timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose() => this.timer?.Dispose();
}
