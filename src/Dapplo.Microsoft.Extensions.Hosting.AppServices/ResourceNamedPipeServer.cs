// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO.Pipes;
using System.Text;
using Microsoft.Extensions.Logging;

#if NET472
using System.Security.AccessControl;
using System.Security.Principal;
#endif

namespace Dapplo.Microsoft.Extensions.Hosting.AppServices
{
    /// <summary>
    ///     This class is the server resource for a named pipe
    /// </summary>
    public sealed class ResourceNamedPipeServer : BaseResourceNamedPipe, IDisposable
    {
        private byte[] _buffer = new byte[32];
        private object _lockingObject = new object();
        private bool _isStopping = false;
        private NamedPipeServerStream _applicationNamedPipe;

        private ResourceNamedPipeServer(ILogger logger, string namedPipeId, string resourceName = null) : base(logger, namedPipeId, resourceName)
        {
        }

        /// <summary>
        ///   This event is fired when a named pipe client is connected
        /// </summary>
        public event EventHandler Connected;

        /// <summary>
        ///     Create a ResourceNamedPipeServer for the specified named pipe id and resource-name
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="namedPipeId">ID of the named pipe, preferably a Guid as string</param>
        /// <param name="resourceName">Name of the resource to lock, e.g your application name, useful for logs</param>
        /// <param name="global">true to have a global named pipe see: https://msdn.microsoft.com/en-us/library/bwe34f1k.aspx</param>
        public static ResourceNamedPipeServer Create(ILogger logger, string namedPipeId, string resourceName = null, bool global = false)
        {
            if (namedPipeId == null)
            {
                throw new ArgumentNullException(nameof(namedPipeId));
            }
            logger ??= new LoggerFactory().CreateLogger<ResourceNamedPipeServer>();
            var applicationNamedPipeServer = new ResourceNamedPipeServer(logger, (global ? @"Global\" : @"Local\") + namedPipeId, resourceName);
            applicationNamedPipeServer.Start();
            return applicationNamedPipeServer;
        }

        /// <summary>
        ///     Starts the named pipe server
        /// </summary>
        public void Start()
        {
            if (_applicationNamedPipe != null)
                throw new InvalidOperationException("Cannot start a ResourceNamedPipeServer when it is already started");

            if (_isStopping)
                throw new InvalidOperationException("Cannot start a ResourceNamedPipeServer when it is already stopped");

            _applicationNamedPipe = new NamedPipeServerStream(_namedPipeId, PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);

            try
            {
                StartWaitingForConnection();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying to begin listening for a connection on named pipe");
                throw;
            }
        }

        private void StartWaitingForConnection()
        {
            _applicationNamedPipe.BeginWaitForConnection(WaitForConnectionCallBack, null);
        }

        private void WaitForConnectionCallBack(IAsyncResult result)
        {
            if (!_isStopping)
            {
                lock (_lockingObject)
                {
                    if (!_isStopping)
                    {
                        // Call EndWaitForConnection to complete the connection operation
                        _applicationNamedPipe.EndWaitForConnection(result);

                        _applicationNamedPipe.BeginRead(_buffer, 0, 32, BeginReadCallback, null);
                    }
                }
            }
        }

        private void BeginReadCallback(IAsyncResult result)
        {
            var readBytes = _applicationNamedPipe.EndRead(result);
            if (readBytes > 0)
            {
                var message = Encoding.ASCII.GetString(_buffer, 0, readBytes);
                if(message == StartedMagicString)
                {
                    Connected?.Invoke(this, EventArgs.Empty);
                }
            }

            if (!_isStopping)
            {
                _applicationNamedPipe.Disconnect();
                StartWaitingForConnection();
            }
        }

        /// <summary>
        ///     Stops the named pipe server
        /// </summary>
        public void Stop()
        {
            _isStopping = true;

            if(_applicationNamedPipe == null)
            {
                _logger.LogWarning("NamedPipeServerResource is already stopped");
            }

            try
            {
                if (_applicationNamedPipe.IsConnected)
                {
                    _applicationNamedPipe.Disconnect();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught while trying to disconnect named pipe");
                throw;
            }
            finally
            {
                _applicationNamedPipe.Close();
                _applicationNamedPipe.Dispose();
                _applicationNamedPipe = null;
            }
        }

        /// <summary>
        ///     This disposes the named pipe server
        /// </summary>
        public void Dispose()
        {
            if (_isStopping)
                Stop();
        }
    }
}
