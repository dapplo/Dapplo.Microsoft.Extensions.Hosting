// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Flurl.Http;
using Dapplo.Hosting.Sample.FrameworkLib;

namespace Dapplo.Hosting.Sample.PluginWithDependency
{
    /// <summary>
    /// Just some service to run in the background and use a dependency
    /// </summary>
    internal class MySampleBackgroundService : IHostedService, IDisposable
    {
        private readonly ILogger logger;
        private Timer timer;
        private readonly string uri = "https://nu.nl";

        public MySampleBackgroundService(ILogger<MySampleBackgroundService> logger)
        {
            this.logger = logger;
            SomeStaticExampleClass.RegisteredServices.Add(nameof(MySampleBackgroundService));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("MySampleBackgroundService is starting.");

            this.timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            this.logger.LogInformation("Known registered Services {0}", string.Join(", ", SomeStaticExampleClass.RegisteredServices));
            this.logger.LogInformation("Retrieving something.");
            Task.Run(async () =>
            {
                try
                {
                    var result = await this.uri.GetStringAsync();
                    this.logger.LogInformation("{0} : {1}", this.uri, result.Substring(0, 40));
                }
                catch (Exception)
                {
                    this.logger.LogError("Couldn't connect to {0}, this was expected behind a corporate firewall, as HttpClient doesn't have a default proxy!", this.uri);
                }
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("MySampleBackgroundService is stopping.");

            this.timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            this.timer?.Dispose();
        }
    }
}
