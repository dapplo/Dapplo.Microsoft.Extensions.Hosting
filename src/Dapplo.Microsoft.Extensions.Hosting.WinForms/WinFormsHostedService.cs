// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dapplo.Microsoft.Extensions.Hosting.WinForms
{
    /// <summary>
    /// This hosts a WinForms service, making sure the lifecycle is managed
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class WinFormsHostedService : IHostedService
    {
        private readonly ILogger<WinFormsHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IWinFormsContext _winFormsContext;
        
        /// <summary>
        /// The constructor which takes all the DI objects
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="serviceProvider">IServiceProvider</param>
        /// <param name="winFormsContext">IWinFormsContext</param>
        public WinFormsHostedService(ILogger<WinFormsHostedService> logger, IServiceProvider serviceProvider, IWinFormsContext winFormsContext)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _winFormsContext = winFormsContext;
            
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _winFormsContext.StartUi(_serviceProvider);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            
            if (_winFormsContext.IsRunning)
            {
                _logger.LogDebug("Stopping WinForms application.");
                await _winFormsContext.Dispatcher.InvokeAsync(()=>
                {
                    // Graceful close, otherwise finalizes try to dispose forms.
                    foreach (var form in Application.OpenForms.Cast<Form>().ToList())
                    {
                        try
                        {
                            form.Close();
                            form.Dispose();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Couldn't cleanup a Form");
                        }
                    }
                    Application.ExitThread();
                });
            }
        }
    }
}
