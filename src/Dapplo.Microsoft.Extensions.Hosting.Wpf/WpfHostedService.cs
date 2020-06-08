// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Dapplo.Microsoft.Extensions.Hosting.Wpf.Internals;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dapplo.Microsoft.Extensions.Hosting.Wpf
{
    /// <summary>
    /// This hosts a WPF service, making sure the lifecycle is managed
    /// </summary>
    public class WpfHostedService : IHostedService
    {
        private readonly ILogger<WpfHostedService> logger;
        private readonly WpfThread wpfThread;
        private readonly IWpfContext wpfContext;

        /// <summary>
        /// The constructor which takes all the DI objects
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="wpfThread">WpfThread</param>
        /// <param name="wpfContext">IWpfContext</param>
        public WpfHostedService(ILogger<WpfHostedService> logger, WpfThread wpfThread, IWpfContext wpfContext)
        {
            this.logger = logger;
            this.wpfThread = wpfThread;
            this.wpfContext = wpfContext;
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.CompletedTask;
            }
            
            // Make the UI thread go
            this.wpfThread.Start();
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (this.wpfContext.IsRunning)
            {
                this.logger.LogDebug("Stopping WPF due to application exit.");
                // Stop application
                await this.wpfContext.Dispatcher.InvokeAsync(() => this.wpfContext.WpfApplication.Shutdown());
            }
        }
    }
}
