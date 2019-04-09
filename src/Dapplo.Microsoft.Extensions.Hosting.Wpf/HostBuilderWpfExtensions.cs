//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2019 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Microsoft.Extensions.Hosting
// 
//  Dapplo.Microsoft.Extensions.Hosting is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Microsoft.Extensions.Hosting is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Microsoft.Extensions.Hosting. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dapplo.Microsoft.Extensions.Hosting.Wpf
{
    /// <summary>
    /// This contains the WPF extensions for Microsoft.Extensions.Hosting 
    /// </summary>
    public static class HostBuilderWpfExtensions
    {
        /// <summary>
        /// Configure an WPF application
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <typeparam name="TShell">IShell Window</typeparam>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder ConfigureWpf<TShell>(this IHostBuilder hostBuilder) where TShell : Window, IShell
        {
            hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
            {
                serviceCollection.AddSingleton<IShell, TShell>();
                serviceCollection.AddHostedService<WpfHostedService>();
            });
            return hostBuilder;
        }
    }
}