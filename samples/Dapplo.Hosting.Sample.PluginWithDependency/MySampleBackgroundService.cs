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
        private readonly ILogger _logger;
        private Timer _timer;
        private readonly string _uri = "https://nu.nl";

        public MySampleBackgroundService(ILogger<MySampleBackgroundService> logger)
        {
            _logger = logger;
            SomeStaticExampleClass.RegisteredServices.Add(nameof(MySampleBackgroundService));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("MySampleBackgroundService is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Known registered Services {0}", string.Join(", ", SomeStaticExampleClass.RegisteredServices));
            _logger.LogInformation("Retrieving something.");
            Task.Run(async () =>
            {
                try
                {
                    var result = await _uri.GetStringAsync();
                    _logger.LogInformation("{0} : {1}", _uri, result.Substring(0, 40));
                }
                catch (Exception)
                {
                    _logger.LogError("Couldn't connect to {0}, this was expected behind a corporate firewall, as HttpClient doesn't have a default proxy!", _uri);
                }
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("MySampleBackgroundService is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
