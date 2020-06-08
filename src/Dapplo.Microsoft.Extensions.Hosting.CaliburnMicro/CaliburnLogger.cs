// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Caliburn.Micro;
using Microsoft.Extensions.Logging;

namespace Dapplo.Microsoft.Extensions.Hosting.CaliburnMicro
{
    /// <summary>
    ///     A logger for Caliburn which logs to Microsoft.Extensions.Logger.ILogger
    /// </summary>
    public class CaliburnLogger : ILog
    {
        private readonly ILogger log;

        /// <summary>
        ///     The constructor is called from the LogManager.GetLog function with the type to log for
        /// </summary>
        /// <param name="logger">ILogger</param>
        public CaliburnLogger(ILogger logger)
        {
            this.log = logger;
        }

        /// <summary>
        ///     Log an error
        /// </summary>
        /// <param name="exception"></param>
        public void Error(Exception exception)
        {
            this.log.LogError(exception, null);
        }

        /// <summary>
        ///     Log information, this is actually reduced to the Dapplo-Level debug as Caliburn speaks a lot!
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void Info(string format, params object[] args)
        {
            // Pre-format the message, otherwise we get problems with dependency objects etc
            this.log.LogDebug(format, args);
        }

        /// <summary>
        ///     Log warning
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void Warn(string format, params object[] args)
        {
            // Pre-format the message, otherwise we get problems with dependency objects etc
            this.log.LogWarning(format, args);
        }
    }
}