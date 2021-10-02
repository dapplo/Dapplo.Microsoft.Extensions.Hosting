// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Dapplo.Microsoft.Extensions.Hosting.WinUI
{
    /// <summary>
    /// Interface used for configuring WinUI 
    /// </summary>
    public interface IWinUIBuilder
    {
        /// <summary>
        /// Action to configure the WinUI context
        /// </summary>
        Action<IWinUIContext> ConfigureContextAction { get; set; }

        /// <summary>
        /// Type of the windows that will be used
        /// </summary>
        IList<Type> WindowTypes { get; }
    }
}
