//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2019 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Microsoft.Extensions.Hosting
// 
//  Dapplo.Microsoft.Extensions.Hosting is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Microsoft.Extensions.Hosting is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Microsoft.Extensions.Hosting. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

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
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IApplicationLifetime _appLifetime;
        private readonly MutexConfig _mutexConfig;
        private ResourceMutex _resourceMutex;

        public MutexLifetimeService(ILogger<MutexLifetimeService> logger, IHostingEnvironment hostingEnvironment, IApplicationLifetime appLifetime, MutexConfig mutexConfig)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _appLifetime = appLifetime;
            _mutexConfig = mutexConfig;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _resourceMutex = ResourceMutex.Create(null, _mutexConfig.MutexId, _hostingEnvironment.ApplicationName, _mutexConfig.IsGlobal);

            _appLifetime.ApplicationStopping.Register(OnStopping);
            if (!_resourceMutex.IsLocked)
            {
                _mutexConfig.WhenNotFirstInstance?.Invoke(_hostingEnvironment);
                _logger.LogDebug("Application {0} already running, stopping application.", _hostingEnvironment.ApplicationName);
                _appLifetime.StopApplication();
            }

            return Task.CompletedTask;
        }


        private void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called, closing mutex.");
            _resourceMutex.Dispose();
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}