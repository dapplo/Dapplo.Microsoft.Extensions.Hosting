using System;
using Dapplo.Microsoft.Extensions.Hosting.ReactiveUI.Wpf;
using Microsoft.Extensions.Hosting;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace Dapplo.Microsoft.Extensions.Hosting.ReactiveUI;

/// <summary>
/// This contains the ReactiveUi extensions for Microsoft.Extensions.Hosting.
/// </summary>
public static class HostBuilderReactiveUiExtensions
{
    /// <summary>
    /// Configure a ReactiveUI application.
    /// </summary>
    /// <param name="hostBuilder">IHostBuilder.</param>
    /// <returns>I Host Builder.</returns>
    public static IHostBuilder ConfigureSplatForMicrosoftDependencyResolver(this IHostBuilder hostBuilder) =>
        InternalBuilderReactiveUiUtility.ConfigureSplatForMicrosoftDependencyResolver(hostBuilder);
}
