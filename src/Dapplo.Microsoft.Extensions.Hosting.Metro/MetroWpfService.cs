// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;

namespace Dapplo.Microsoft.Extensions.Hosting.Metro;

/// <summary>
/// This configures WPF to use Metro
/// </summary>
public class MetroWpfService : IWpfService
{
    private readonly IMetroContext metroContext;

    /// <summary>
    /// The constructor which takes all the DI objects
    /// </summary>
    /// <param name="metroContext">IMetroContext</param>
    public MetroWpfService(IMetroContext metroContext) => this.metroContext = metroContext;

    /// <inheritdoc />
    public void Initialize(Application application)
    {
        var resourceDictionary = new ResourceDictionary();

        foreach(var resource in this.metroContext.Resources)
        {
            resourceDictionary.Source = resource;
            application.Resources.MergedDictionaries.Add(resourceDictionary);
        }

        foreach (var style in this.metroContext.Styles)
        {
            resourceDictionary.Source = new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/{style}.xaml");
            application.Resources.MergedDictionaries.Add(resourceDictionary);
        }
        resourceDictionary.Source = new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Themes/{this.metroContext.Theme}.xaml");
        application.Resources.MergedDictionaries.Add(resourceDictionary);
    }

}
