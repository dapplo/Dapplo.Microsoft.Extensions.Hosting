using System.Threading;
using System.Threading.Tasks;
using Dapplo.Microsoft.Extensions.Hosting.FrameworkLib;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dapplo.Microsoft.Extensions.Hosting.Plugins.PluginOriginalSample
{
    /// <summary>
    /// Example for a IHostedService which tracks LivetimeEvents
    /// </summary>
    internal class LifetimeEventsHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IApplicationLifetime _appLifetime;

        /// <inheritdoc />
        public LifetimeEventsHostedService(ILogger<LifetimeEventsHostedService> logger, IApplicationLifetime appLifetime)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            SomeStaticExampleClass.RegisteredServices.Add(nameof(LifetimeEventsHostedService));
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");

            // Perform post-startup activities here
        }

        private void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");

            // Perform on-stopping activities here
        }

        private void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");

            // Perform post-stopped activities here
        }
    }
}