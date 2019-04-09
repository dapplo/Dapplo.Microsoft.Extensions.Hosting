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

using System;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.AppServices.Internal
{
    /// <summary>
    /// This is the configuration for the mutex service
    /// </summary>
    internal class MutexConfig
    {
        /// <summary>
        /// The name of the mutex, usually a GUID
        /// </summary>
        public string MutexId { get; set; }
        
        /// <summary>
        /// This decides what prefix the mutex name gets, true will prepend Global\ and false Local\
        /// </summary>
        public bool IsGlobal { get; set; }

        /// <summary>
        /// The action which is called when the mutex cannot be locked
        /// </summary>
        public Action<IHostingEnvironment> WhenNotFirstInstance { get; set; }
    }
}
