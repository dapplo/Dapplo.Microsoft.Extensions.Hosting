using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace Dapplo.Microsoft.Extensions.Hosting.ReactiveUI.Wpf;

/// <summary>
/// This contains the ReactiveUi extensions for Microsoft.Extensions.Hosting for Splat.
/// </summary>
public static class SplatHostExtensions
{
    /// <summary>
    /// Maps the splat locator to the IServiceProvider.
    /// </summary>
    /// <param name="host">The host.</param>
    /// <param name="containerFactory">The IServiceProvider factory.</param>
    /// <returns>A Value.</returns>
    public static IHost MapSplatLocator(this IHost host, Action<IServiceProvider> containerFactory)
    {
        var c = host.Services;
        c.UseMicrosoftDependencyResolver();
        containerFactory.Invoke(c);
        return host;
    }
}
