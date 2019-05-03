// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Dapplo.Microsoft.Extensions.Hosting.Metro.Internals
{
    /// <inheritdoc />
    public class MetroContext : IMetroContext
    {
        /// <inheritdoc />
        public List<Uri> Resources { get; } = new List<Uri>();

        /// <inheritdoc />
        public List<string> Styles { get; } = new List<string>();

        /// <inheritdoc />
        public string Theme { get; set; } = "Light.Blue";
    }
}