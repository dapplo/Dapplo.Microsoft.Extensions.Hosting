// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.Microsoft.Extensions.Hosting.WinUI.Internals;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Dapplo.Microsoft.Extensions.Hosting.WinUI
{
    /// <summary>
    /// This hosts a WinUI service, making sure the lifecycle is managed
    /// </summary>
    public class WinUIHostedService : IHostedService
    {
        private readonly ILogger<WinUIHostedService> logger;
        private readonly IWinUIContext winUIContext;
        private readonly WinUIThread winUIThread;

        /// <summary>
        /// The constructor which takes all the DI objects
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="winUIThread">WinUIThread</param>
        /// <param name="winUIContext">IWinUIContext</param>
        public WinUIHostedService(ILogger<WinUIHostedService> logger, WinUIThread winUIThread, IWinUIContext winUIContext)
        {
            this.logger = logger;
            this.winUIThread = winUIThread;
            this.winUIContext = winUIContext;
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.CompletedTask;
            }

            // Make the UI thread go
            this.winUIThread.Start();
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (this.winUIContext.IsRunning)
            {
                this.logger.LogDebug("Stopping WinUI due to application exit.");
                // Stop application
                TaskCompletionSource completion = new();
                this.winUIContext.Dispatcher.TryEnqueue(() =>
                {
                    this.winUIContext.WinUIApplication.Exit();
                    completion.SetResult();
                });
                await completion.Task;
            }
        }
    }
}
