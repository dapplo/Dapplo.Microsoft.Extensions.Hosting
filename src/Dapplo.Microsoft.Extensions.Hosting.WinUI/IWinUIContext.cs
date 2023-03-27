// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dapplo.Microsoft.Extensions.Hosting.UiThread;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;

namespace Dapplo.Microsoft.Extensions.Hosting.WinUI
{
    /// <summary>
    /// The WinUI context contains all information about the WinUI application and how it's started and stopped
    /// </summary>
    public interface IWinUIContext : IUiContext
    {
        /// <summary>
        /// Started instance of <see cref="AppWindowType"/>
        /// </summary>
        Window AppWindow { get; set; }

        /// <summary>
        /// App Window type.
        /// </summary>
        Type AppWindowType { get; set; }

        /// <summary>
        /// This WinUI dispatcher
        /// </summary>
        DispatcherQueue Dispatcher { get; set; }

        /// <summary>
        /// The Application
        /// </summary>
        Application WinUIApplication { get; set; }
    }
}
