// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Dapplo.Microsoft.Extensions.Hosting.CaliburnMicro;

/// <summary>
/// A ViewModel can supply settings for a dialog or window
/// </summary>
public interface IHaveSettings
{
    /// <summary>
    /// A IDictionary with properties for the window or dialog
    /// </summary>
    IDictionary<string, object> Settings { get; }
}