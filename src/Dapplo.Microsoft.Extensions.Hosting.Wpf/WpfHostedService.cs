// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dapplo.Microsoft.Extensions.Hosting.Wpf
{
    /// <summary>
    /// This hosts a WPF service, making sure the lifecycle is managed
    /// </summary>
    public class WpfHostedService : IHostedService
    {
        private readonly ILogger<WpfHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IWpfContext _wpfContext;

        /// <summary>
        /// The constructor which takes all the DI objects
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="serviceProvider">IServiceProvider</param>
        /// <param name="wpfContext">IWpfContext</param>
        public WpfHostedService(ILogger<WpfHostedService> logger, IServiceProvider serviceProvider, IWpfContext wpfContext)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _wpfContext = wpfContext;
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Make the UI thread go
            _wpfContext.StartUi(_serviceProvider);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_wpfContext.IsRunning)
            {
                _logger.LogDebug("Stopping WPF due to application exit.");
                // Stop application
                await _wpfContext.Dispatcher.InvokeAsync(() => _wpfContext.WpfApplication.Shutdown());
            }
        }
    }
}
