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

using System.Windows;

namespace Dapplo.Microsoft.Extensions.Hosting.Wpf
{
    /// <summary>
    /// The WPF context contains all information about the WPF application and how it's started and stopped
    /// </summary>
    public interface IWpfContext
    {
        /// <summary>
        /// This is the WPF ShutdownMode used for the WPF application lifetime, default is OnLastWindowClose
        /// </summary>
        ShutdownMode ShutdownMode { get; set; }
        
        /// <summary>
        /// Defines if the host application is stopped when the WPF applications stops
        /// </summary>
        bool IsLifetimeLinked { get; set; }
        
        /// <summary>
        /// Is the WPF application started and still running? 
        /// </summary>
        bool IsRunning { get; set; }
        
        /// <summary>
        /// The Application
        /// </summary>
        Application WpfApplication { get; set; }
    }
}