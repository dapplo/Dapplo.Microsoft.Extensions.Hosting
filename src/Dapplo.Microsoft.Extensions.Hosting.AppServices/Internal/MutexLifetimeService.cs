// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dapplo.Microsoft.Extensions.Hosting.AppServices.Internal
{
    /// <summary>
    /// This maintains the mutex lifetime
    /// </summary>
    internal class MutexLifetimeService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IMutexBuilder _mutexBuilder;
        private ResourceMutex _resourceMutex;
        private ResourceNamedPipeServer _resourceNamedPipeServer;

        public MutexLifetimeService(ILogger<MutexLifetimeService> logger, IHostEnvironment hostEnvironment, IHostApplicationLifetime hostApplicationLifetime, IMutexBuilder mutexBuilder)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            _hostApplicationLifetime = hostApplicationLifetime;
            _mutexBuilder = mutexBuilder;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _resourceMutex = ResourceMutex.Create(null, _mutexBuilder.MutexId, _hostEnvironment.ApplicationName, _mutexBuilder.IsGlobal);

            _hostApplicationLifetime.ApplicationStopping.Register(OnStopping);
            if (!_resourceMutex.IsLocked)
            {
                _mutexBuilder.WhenNotFirstInstance?.Invoke(_hostEnvironment, _logger);
                _logger.LogDebug("Application {0} already running, notifying other instance.", _hostEnvironment.ApplicationName);

                try
                {
                    using var client = await ResourceNamedPipeClient.Create(null, _mutexBuilder.MutexId, _hostEnvironment.ApplicationName, _mutexBuilder.IsGlobal);
                }
                catch(Exception e)
                {
                    _logger.LogWarning(e, "Error while trying to nofity other instance");
                }

                _logger.LogDebug("stopping application.");
                _hostApplicationLifetime.StopApplication();
            }
            else
            {
                _logger.LogDebug("This is the first instance of application {0}, creating named pipe server.", _hostEnvironment.ApplicationName);

                _resourceNamedPipeServer = ResourceNamedPipeServer.Create(null, _mutexBuilder.MutexId, _hostEnvironment.ApplicationName, _mutexBuilder.IsGlobal);
                _resourceNamedPipeServer.Connected += delegate
                {
                    _mutexBuilder.WhenOtherInstanceStarts?.Invoke(_hostEnvironment, _logger);
                };
            }
        }

        private void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called, closing named pipe.");
            if(_resourceNamedPipeServer != null)
            {
                _resourceNamedPipeServer.Dispose();
            }

            _logger.LogInformation("Also closing mutex.");
            _resourceMutex.Dispose();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}