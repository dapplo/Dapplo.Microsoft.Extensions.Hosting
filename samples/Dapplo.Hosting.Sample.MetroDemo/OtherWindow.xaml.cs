// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;
using Microsoft.Extensions.Logging;

namespace Dapplo.Hosting.Sample.MetroDemo
{
    /// <summary>
    /// A simple example WPF window
    /// </summary>
    public partial class OtherWindow
    {
        private readonly ILogger<OtherWindow> logger;

        public OtherWindow(ILogger<OtherWindow> logger)
        {
            InitializeComponent();
            this.logger = logger;
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.logger.LogDebug("Closing OtherWindow");
            Close();
        }
    }
}
