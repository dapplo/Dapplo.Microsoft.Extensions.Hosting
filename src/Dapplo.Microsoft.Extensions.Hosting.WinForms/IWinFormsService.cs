// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dapplo.Microsoft.Extensions.Hosting.WinForms
{
    /// <summary>
    /// This defines a service which is called before the message loop is started
    /// </summary>
    public interface IWinFormsService
    {
        /// <summary>
        /// Do whatever you need to do to initialize, this is called from the UI thread
        /// </summary>
        void Initialize();
    }
}
