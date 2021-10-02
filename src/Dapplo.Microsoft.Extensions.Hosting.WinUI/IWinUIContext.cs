// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.Microsoft.Extensions.Hosting.UiThread;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;

namespace Dapplo.Microsoft.Extensions.Hosting.WinUI
{
    /// <summary>
    /// The WinUI context contains all information about the WinUI application and how it's started and stopped
    /// </summary>
    public interface IWinUIContext : IUiContext
    {
        /// <summary>
        /// This WinUI dispatcher
        /// </summary>
        DispatcherQueue Dispatcher { get; set; }

        /// <summary>
        /// Window Types.
        /// </summary>
        IList<Type> WindowTypes { get; }

        /// <summary>
        /// The Application
        /// </summary>
        Application WinUIApplication { get; set; }
    }
}
