// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Dapplo.Microsoft.Extensions.Hosting.UiThread;

namespace Dapplo.Microsoft.Extensions.Hosting.Avalonia;

/// <summary>
/// The Avalonia context contains all information about the Avalonia application and how it's started and stopped
/// </summary>
public interface IAvaloniaContext : IUiContext
{
    /// <summary>
    /// This is the Avalonia ShutdownMode used for the Avalonia application lifetime, default is OnLastWindowClose
    /// </summary>
    ShutdownMode ShutdownMode { get; set; }

    /// <summary>
    /// The Application
    /// </summary>
    Application AvaloniaApplication { get; set; }

    /// <summary>
    /// The Application Lifetime
    /// </summary>
    IClassicDesktopStyleApplicationLifetime ApplicationLifetime { get; set; }

    /// <summary>
    /// The Avalonia dispatcher
    /// </summary>
    Dispatcher Dispatcher { get; }
}
