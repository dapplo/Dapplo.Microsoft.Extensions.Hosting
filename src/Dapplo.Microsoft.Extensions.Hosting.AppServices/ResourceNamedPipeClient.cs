// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

#if NET472
using System.Security.AccessControl;
using System.Security.Principal;
#endif

namespace Dapplo.Microsoft.Extensions.Hosting.AppServices
{
    /// <summary>
    ///    This class is the client resource of a named pipe
    /// </summary>
    public sealed class ResourceNamedPipeClient : BaseResourceNamedPipe, IDisposable
    {
        private NamedPipeClientStream _applicationNamedPipe;

        private ResourceNamedPipeClient(ILogger logger, string namedPipeId, string resourceName = null) : base(logger, namedPipeId, resourceName)
        {
        }

        /// <summary>
        ///     Create a ResourceNamedPipeServer for the specified named pipe id and resource-name
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="namedPipeId">ID of the named pipe, preferably a Guid as string</param>
        /// <param name="resourceName">Name of the resource to lock, e.g your application name, useful for logs</param>
        /// <param name="global">true to have a global named pipe see: https://msdn.microsoft.com/en-us/library/bwe34f1k.aspx</param>
        public static async Task<ResourceNamedPipeClient> Create(ILogger logger, string namedPipeId, string resourceName = null, bool global = false)
        {
            if (namedPipeId == null)
            {
                throw new ArgumentNullException(nameof(namedPipeId));
            }
            logger ??= new LoggerFactory().CreateLogger<ResourceNamedPipeServer>();
            var applicationNamedPipeClient = new ResourceNamedPipeClient(logger, (global ? @"Global\" : @"Local\") + namedPipeId, resourceName);
            await applicationNamedPipeClient.Connect();
            return applicationNamedPipeClient;
        }

        /// <summary>
        ///     Connects to the server part and sends a signal that we started
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Connect(int timeout = 3000, CancellationToken? cancellationToken = null)
        {
            _applicationNamedPipe = new NamedPipeClientStream(".", _namedPipeId, PipeDirection.InOut, PipeOptions.Asynchronous);
            await  _applicationNamedPipe.ConnectAsync(timeout, cancellationToken ?? CancellationToken.None);
            await _applicationNamedPipe.WriteAsync(Encoding.ASCII.GetBytes(StartedMagicString), cancellationToken ?? CancellationToken.None);
        }

        /// <summary>
        ///     This disposes the named pipe client
        /// </summary>
        public void Dispose()
        {
            if(_applicationNamedPipe != null)
            {
                _applicationNamedPipe.Close();
                _applicationNamedPipe.Dispose();
                _applicationNamedPipe = null;
            }
        }
    }
}
