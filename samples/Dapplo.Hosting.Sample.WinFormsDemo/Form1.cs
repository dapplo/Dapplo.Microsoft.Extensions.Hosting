// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Forms;
using Dapplo.Microsoft.Extensions.Hosting.WinForms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dapplo.Hosting.Sample.WinFormsDemo
{
    /// <summary>
    /// Just a simple form
    /// </summary>
    public partial class Form1 : Form, IWinFormsShell
    {
        private readonly ILogger<Form1> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Form1(ILogger<Form1> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            InitializeComponent();
        }

        private void buttonForm2_Click(object sender, EventArgs e)
        {
            _logger.LogDebug("Show form2 clicked!");
            var form2 = _serviceProvider.GetService<Form2>();
            form2.Show();
        }
        
        private void buttonExit_Click(object sender, EventArgs e)
        {
            _logger.LogDebug("Exit clicked!");
            Application.Exit();
        }
    }
}
