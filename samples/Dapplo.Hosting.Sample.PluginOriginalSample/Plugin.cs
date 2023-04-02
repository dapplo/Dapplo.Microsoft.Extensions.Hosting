// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.Microsoft.Extensions.Hosting.Plugins;

namespace Dapplo.Hosting.Sample.PluginOriginalSample;

/// <summary>
/// This plug-in configures the HostBuilderContext to have the hosted services from the online example
/// </summary>
[PluginOrder(100)]
public class Plugin : PluginBase<LifetimeEventsHostedService, TimedHostedService>
{
}
