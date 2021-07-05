// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Caliburn.Micro;

namespace Dapplo.Hosting.Sample.CaliburnMicroDemo.ViewModels
{
    public class OtherViewModel : Screen
    {
        public Task Exit() => TryCloseAsync();
    }
}
