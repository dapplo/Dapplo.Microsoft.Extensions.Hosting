// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;

namespace Dapplo.Microsoft.Extensions.Hosting.Avalonia;

/// <summary>
/// Interface used for configuring Avalonia 
/// </summary>
public interface IAvaloniaBuilder
{
    /// <summary>
    /// Type of the application that will be used
    /// </summary>
    Type ApplicationType { get; set; }
    
    /// <summary>
    /// Gets or sets an existing application.
    /// </summary>
    Application Application { get; set; }
    
    /// <summary>
    /// Type of the windows that will be used
    /// </summary>
    IList<Type> WindowTypes { get; }
    
    /// <summary>
    /// Action to configure the Avalonia context
    /// </summary>
    Action<IAvaloniaContext> ConfigureContextAction { get; set; }
    
    /// <summary>
    /// Action to configure the AppBuilder
    /// </summary>
    Action<AppBuilder> ConfigureAppBuilderAction { get; set; }
}
