// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

namespace Dapplo.Microsoft.Extensions.Hosting.CaliburnMicro;

/// <summary>
/// Export with this interface to be able to configure dialog views
/// </summary>
public interface IConfigureDialogViews
{
    /// <summary>
    /// This is called, so you can configure the dialog view
    /// </summary>
    /// <param name="view">Window for the view</param>
    /// <param name="viewModel">object with the ViewModel</param>
    void ConfigureDialogView(Window view, object viewModel = null);
}