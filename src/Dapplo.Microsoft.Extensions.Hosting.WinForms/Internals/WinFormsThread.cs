using System;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.WinForms.Internals
{
    /// <summary>
    /// This contains the logic for the WinForms thread
    /// </summary>
    internal class WinFormsThread
    {
        private readonly ManualResetEvent _serviceManualResetEvent = new ManualResetEvent(false);
        private readonly IWinFormsContext _winFormsContext;
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor which is called from the IWinFormsContext
        /// </summary>
        /// <param name="winFormsContext">IWinFormsContext</param>
        public WinFormsThread(IWinFormsContext winFormsContext)
        {
            _winFormsContext = winFormsContext;
            // Create a thread which runs WPF
            var newWpfThread = new Thread(WinFormsThreadStart)
            {
                IsBackground = true
            };
            // Set the apartment state
            newWpfThread.SetApartmentState(ApartmentState.STA);
            // Start the new WPF thread
            newWpfThread.Start();
        }

        /// <summary>
        /// Start the DI service on the thread
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider</param>
        public void Start(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            // Make the UI thread go
            _serviceManualResetEvent.Set();
        }

        /// <summary>
        /// Start Windows Forms
        /// </summary>
        private void WinFormsThreadStart()
        {
            var currentDispatcher = Dispatcher.CurrentDispatcher;
            _winFormsContext.Dispatcher = currentDispatcher;

            // Create our SynchronizationContext, and install it:
            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(currentDispatcher));

            if (_winFormsContext.EnableVisualStyles)
            {
                Application.EnableVisualStyles();
            }

            // Register to the WinForms application exit to stop the host application
            Application.ApplicationExit += OnApplicationExit;

            // Wait for the startup
            _serviceManualResetEvent.WaitOne();

            // Run the application
            _winFormsContext.IsRunning = true;

            // Use the provided IWinFormsService
            var winFormServices = _serviceProvider.GetServices<IWinFormsService>();
            if (winFormServices != null)
            {
                foreach(var winFormService in winFormServices)
                {
                    winFormService.Initialize();
                }
            }

            if (_serviceProvider.GetService<IWinFormsShell>() is Form formShell)
            {
                Application.Run(formShell);
            }
            else
            {
                Application.Run();
            }
        }

        private void OnApplicationExit(object sender, EventArgs eventArgs)
        {
            Application.ApplicationExit -= OnApplicationExit;

            _winFormsContext.IsRunning = false;
            if (!_winFormsContext.IsLifetimeLinked)
            {
                return;
            }

            //_logger.LogDebug("Stopping host application due to WinForms application exit.");
            _serviceProvider.GetService<IHostApplicationLifetime>().StopApplication();
        }
    }
}