// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;
using Microsoft.Extensions.Logging;

namespace Dapplo.Microsoft.Extensions.Hosting.Metro
{
    /// <summary>
    /// This configures WPF to use Metro
    /// </summary>
    public class MetroWpfService : IWpfService
    {
        private readonly ILogger<MetroWpfService> _logger;
        private readonly IMetroContext _metroContext;

        /// <summary>
        /// The constructor which takes all the DI objects
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="metroContext">IMetroContext</param>
        public MetroWpfService(ILogger<MetroWpfService> logger, IMetroContext metroContext)
        {
            _logger = logger;
            _metroContext = metroContext;
        }

        /// <inheritdoc />
        public void Initialize(Application application)
        {
            ResourceDictionary resourceDictionary = new ResourceDictionary();

            foreach(var resource in _metroContext.Resources)
            {
                resourceDictionary.Source = resource;
                application.Resources.MergedDictionaries.Add(resourceDictionary);
            }

            foreach (var style in _metroContext.Styles)
            {
                resourceDictionary.Source = new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/{style}.xaml");
                application.Resources.MergedDictionaries.Add(resourceDictionary);
            }
            resourceDictionary.Source = new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Themes/{_metroContext.Theme}.xaml");
            application.Resources.MergedDictionaries.Add(resourceDictionary);
        }

    }
}
