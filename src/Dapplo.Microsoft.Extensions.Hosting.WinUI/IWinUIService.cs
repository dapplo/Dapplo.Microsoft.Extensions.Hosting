// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.UI.Xaml;

namespace Dapplo.Microsoft.Extensions.Hosting.WinUI
{
    /// <summary>
    /// This defines a service which is called before the message loop is started
    /// </summary>
    public interface IWinUIService
    {
        /// <summary>
        /// Do whatever you need to do to initialize WinUI, this is called from the UI thread
        /// </summary>
        /// <param name="application">Application</param>
        void Initialize(Application application);
    }
}
