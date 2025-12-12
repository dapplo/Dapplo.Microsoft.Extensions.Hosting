// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;

namespace Dapplo.Microsoft.Extensions.Hosting.Avalonia.Internals;

/// <inheritdoc />
internal class AvaloniaBuilder : IAvaloniaBuilder
{
    /// <inheritdoc />
    public Type ApplicationType { get; set; }

    /// <inheritdoc />
    public Application Application { get; set; }

    /// <inheritdoc />
    public IList<Type> WindowTypes { get; } = new List<Type>();

    /// <inheritdoc />
    public Action<IAvaloniaContext> ConfigureContextAction { get; set; }

    /// <inheritdoc />
    public Action<AppBuilder> ConfigureAppBuilderAction { get; set; }
}
