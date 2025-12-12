// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Avalonia;

namespace Dapplo.Microsoft.Extensions.Hosting.Avalonia;

/// <summary>
/// This defines a service which is called before the message loop is started
/// </summary>
public interface IAvaloniaService
{
    /// <summary>
    /// Do whatever you need to do to initialize Avalonia, this is called from the UI thread
    /// </summary>
    /// <param name="application">Application</param>
    void Initialize(Application application);
}
