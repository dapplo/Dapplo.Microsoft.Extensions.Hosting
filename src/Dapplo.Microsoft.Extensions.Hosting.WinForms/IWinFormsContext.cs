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

using System.Windows.Threading;

namespace Dapplo.Microsoft.Extensions.Hosting.WinForms
{
    /// <summary>
    /// The WinForms context contains all information about the WinForms application and how it's started and stopped
    /// </summary>
    public interface IWinFormsContext
    {
        /// <summary>
        /// Defines if the host application is stopped when the WinForms applications stops
        /// </summary>
        bool IsLifetimeLinked { get; set; }
        
        /// <summary>
        /// Is the WinForms application started and still running? 
        /// </summary>
        bool IsRunning { get; set; }
        
        /// <summary>
        /// The dispatcher to send information to forms
        /// </summary>
        Dispatcher FormsDispatcher { get; }
    }
}