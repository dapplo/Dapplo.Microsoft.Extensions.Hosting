// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

#if NET472
using System.Security.AccessControl;
using System.Security.Principal;
#endif

namespace Dapplo.Microsoft.Extensions.Hosting.AppServices
{
    /// <summary>
    ///     This is a base class for named pipe resources
    /// </summary>
    public abstract class BaseResourceNamedPipe
    {
        protected const string StartedMagicString = "__STARTED__";
        protected readonly ILogger _logger;
        protected readonly string _namedPipeId;
        protected readonly string _resourceName;

        /// <summary>
        ///     Protected constructor
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="namedPipeId">string with a unique Named Pipe ID</param>
        /// <param name="resourceName">optional name for the resource</param>
        protected BaseResourceNamedPipe(ILogger logger, string namedPipeId, string resourceName = null)
        {
            _logger = logger;
            _namedPipeId = namedPipeId;
            _resourceName = resourceName ?? namedPipeId;
        }
    }
}
