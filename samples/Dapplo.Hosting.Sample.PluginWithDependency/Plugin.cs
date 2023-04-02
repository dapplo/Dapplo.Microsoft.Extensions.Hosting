// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.Microsoft.Extensions.Hosting.Plugins;

namespace Dapplo.Hosting.Sample.PluginWithDependency;

/// <summary>
/// A simple plugin definition, registering the BackgroundService
/// </summary>
[PluginOrder(-1)]
public class Plugin : PluginBase<MySampleBackgroundService>
{
}
