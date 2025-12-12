// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Dapplo.Microsoft.Extensions.Hosting.AppServices.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.AppServices;

/// <summary>
/// Extensions for loading plugins
/// </summary>
public static class HostBuilderApplicationExtensions
{
    /// <summary>
    /// Prevent that an application runs multiple times
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <param name="configureAction">Action to configure IMutexBuilder</param>
    /// <returns>IHostBuilder for fluently calling</returns>
    public static IHostBuilder ConfigureSingleInstance(this IHostBuilder hostBuilder, Action<IMutexBuilder> configureAction) =>
        InternalBuilderAppServiceUtility.ConfigureSingleInstance(hostBuilder, configureAction);

    /// <summary>
    /// Prevent that an application runs multiple times
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder</param>
    /// <param name="mutexId">string</param>
    /// <returns>IHostBuilder for fluently calling</returns>
    public static IHostBuilder ConfigureSingleInstance(this IHostBuilder hostBuilder, string mutexId) =>
        InternalBuilderAppServiceUtility.ConfigureSingleInstance(hostBuilder, mutexId);
}
