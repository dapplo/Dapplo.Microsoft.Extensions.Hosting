using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Dapplo.Hosting.Sample.WpfDemo
{
    public class WpfHostedService : IHostedService
    {
        private readonly Window _shell;
        private readonly IApplicationLifetime _applicationLifetime;
        private Application _application;
        private TaskScheduler _taskScheduler;

        public WpfHostedService(IApplicationLifetime applicationLifetime, IShell shell = null)
        {
            _shell = shell as Window;
            _applicationLifetime = applicationLifetime;
            
            _taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _applicationLifetime.ApplicationStopping.Register(()  =>
            {
                _application.Dispatcher.Invoke(() => _application.Shutdown());
            });

            _application = new Application
            {
                ShutdownMode = ShutdownMode.OnLastWindowClose
            };
            _application.Exit += (s,e) =>
            {
                _applicationLifetime.StopApplication();
            };

            Task.Factory.StartNew(() =>
            {
                if (_shell != null)
            {
                _application.Run(_shell);
            }
            else
            {
                _application.Run();
            }
            }, default, TaskCreationOptions.DenyChildAttach, _taskScheduler);
            
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Task.Factory.StartNew(() =>
            {
                _application.Shutdown();
            }, default, TaskCreationOptions.DenyChildAttach, _taskScheduler);
            return Task.CompletedTask;
        }
    }
}
