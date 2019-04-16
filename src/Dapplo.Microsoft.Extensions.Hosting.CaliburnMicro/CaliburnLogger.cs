//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2018 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.CaliburnMicro
// 
//  Dapplo.CaliburnMicro is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.CaliburnMicro is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.CaliburnMicro. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#region using

using System;
using Caliburn.Micro;
using Microsoft.Extensions.Logging;

#endregion

namespace Dapplo.Microsoft.Extensions.Hosting.CaliburnMicro
{
    /// <summary>
    ///     A logger for Caliburn
    /// </summary>
    public class CaliburnLogger : ILog
    {
        private readonly ILogger _log;

        /// <summary>
        ///     The constructor is called from the LogManager.GetLog function with the type to log for
        /// </summary>
        /// <param name="logger">ILogger</param>
        public CaliburnLogger(ILogger logger)
        {
            _log = logger;
        }

        /// <summary>
        ///     Log an error
        /// </summary>
        /// <param name="exception"></param>
        public void Error(Exception exception)
        {
            _log.LogError(exception, null);
        }

        /// <summary>
        ///     Log information, this is actually reduced to the Dapplo-Level debug as Caliburn speaks a lot!
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void Info(string format, params object[] args)
        {
            // Pre-format the message, otherwise we get problems with dependency objects etc
            _log.LogDebug(format, args);
        }

        /// <summary>
        ///     Log warning
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void Warn(string format, params object[] args)
        {
            // Pre-format the message, otherwise we get problems with dependency objects etc
            _log.LogWarning(format, args);
        }
    }
}