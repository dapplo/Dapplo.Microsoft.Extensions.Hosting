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
public static class HostApplicationBuilderCaliburnMicroExtensions
{
    

    /// <summary>
    /// Configure Caliburn.Micro
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <returns>IHostBuilder</returns>
    public static IHostApplicationBuilder ConfigureCaliburnMicro(this IHostApplicationBuilder hostApplicationBuilder) =>
        InternalBuilderCaliburnMicroUtility.ConfigureCaliburnMicro(hostApplicationBuilder);

    /// <summary>
    /// Configure Caliburn.Micro with the shell
    /// </summary>
    /// <param name="hostApplicationBuilder">IHostApplicationBuilder</param>
    /// <returns>IHostApplicationBuilder</returns>
    public static IHostApplicationBuilder ConfigureCaliburnMicro<TShell>(this IHostApplicationBuilder hostApplicationBuilder)
        where TShell : class, ICaliburnMicroShell =>
        InternalBuilderCaliburnMicroUtility.ConfigureCaliburnMicro<TShell>(hostApplicationBuilder);
}
