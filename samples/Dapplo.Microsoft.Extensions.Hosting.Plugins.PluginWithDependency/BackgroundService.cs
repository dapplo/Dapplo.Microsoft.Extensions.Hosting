using System;
using System.Threading;
using System.Threading.Tasks;
using Dapplo.HttpExtensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dapplo.Microsoft.Extensions.Hosting.Plugins.PluginWithDependency
{
    internal class BackgroundService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private readonly Uri _uri = new Uri("https://nu.nl");

        public BackgroundService(ILogger<BackgroundService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Retrieving something.");

            Task.Run(async () =>
            {
                try
                {
                    var result = await (_uri.GetAsAsync<string>());
                    _logger.LogInformation("{0} : {1}", _uri, result.Substring(0, 40));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Couldn't connect to {0}", _uri);
                }
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
