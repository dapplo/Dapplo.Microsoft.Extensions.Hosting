// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Caliburn.Micro;
using Dapplo.Microsoft.Extensions.Hosting.CaliburnMicro.Extensions;
using Dapplo.Microsoft.Extensions.Hosting.CaliburnMicro.Internals;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.CaliburnMicro;

/// <summary>
/// This contains the CaliburnMicro extensions for Microsoft.Extensions.Hosting
/// </summary>
public static class HostBuilderCaliburnMicroExtensions
{
    /// <summary>
    /// Configure Caliburn.Micro
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <returns>IHostBuilder</returns>
    public static IHostBuilder ConfigureCaliburnMicro(this IHostBuilder hostBuilder) =>
        InternalBuilderCaliburnMicroUtility.ConfigureCaliburnMicro(hostBuilder);

    /// <summary>
    /// Configure Caliburn.Micro with the shell
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <returns>IHostBuilder</returns>
    public static IHostBuilder ConfigureCaliburnMicro<TShell>(this IHostBuilder hostBuilder)
        where TShell : class, ICaliburnMicroShell =>
        InternalBuilderCaliburnMicroUtility.ConfigureCaliburnMicro<TShell>(hostBuilder);
}
