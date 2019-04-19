// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dapplo.Microsoft.Extensions.Hosting.AppServices
{
    /// <summary>
    /// This is to configure the ForceSingleInstance extension
    /// </summary>
    public interface IMutexBuilder
    {
        /// <summary>
        /// The name of the mutex, usually a GUID
        /// </summary>
        string MutexId { get; set; }

        /// <summary>
        /// This decides what prefix the mutex name gets, true will prepend Global\ and false Local\
        /// </summary>
        bool IsGlobal { get; set; }

        /// <summary>
        /// The action which is called when the mutex cannot be locked
        /// </summary>
        Action<IHostingEnvironment, ILogger> WhenNotFirstInstance { get; set; }
    }
}