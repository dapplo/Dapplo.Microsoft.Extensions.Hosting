// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;

namespace Dapplo.Microsoft.Extensions.Hosting.Avalonia.Internals;

/// <inheritdoc />
internal class AvaloniaContext : IAvaloniaContext
{
    /// <inheritdoc />
    public ShutdownMode ShutdownMode { get; set; } = ShutdownMode.OnLastWindowClose;

    /// <inheritdoc />
    public bool IsLifetimeLinked { get; set; }

    /// <inheritdoc />
    public bool IsRunning { get; set; }

    /// <inheritdoc />
    public Application AvaloniaApplication { get; set; }

    /// <inheritdoc />
    public IClassicDesktopStyleApplicationLifetime ApplicationLifetime { get; set; }

    /// <inheritdoc />
    public Dispatcher Dispatcher => Dispatcher.UIThread;
}
