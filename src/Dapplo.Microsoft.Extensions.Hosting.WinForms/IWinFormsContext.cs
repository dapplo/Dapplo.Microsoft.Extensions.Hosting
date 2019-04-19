// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
        /// Specify if the visual styles need to be set, default is true
        /// </summary>
        bool EnableVisualStyles { get; set; }
        
        /// <summary>
        /// Is the WinForms application started and still running? 
        /// </summary>
        bool IsRunning { get; set; }
        
        /// <summary>
        /// The dispatcher to send information to forms
        /// </summary>
        Dispatcher FormsDispatcher { get; set; }
    }
}