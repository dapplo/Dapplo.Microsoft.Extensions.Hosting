// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dapplo.Microsoft.Extensions.Hosting.Metro;

/// <summary>
/// Extensions to modify the IMetroContext
/// </summary>
public static class MetroContextExtensions
{
    /// <summary>
    /// Add a resource
    /// </summary>
    /// <param name="metroContext"></param>
    /// <param name="resource"></param>
    /// <returns>IMetroContext</returns>
    public static IMetroContext AddResource(this IMetroContext metroContext, Uri resource)
    {
        metroContext.Resources.Add(resource);
        return metroContext;
    }

    /// <summary>
    /// Add a resource
    /// </summary>
    /// <param name="metroContext"></param>
    /// <param name="style">string</param>
    /// <returns>IMetroContext</returns>
    public static IMetroContext AddStyle(this IMetroContext metroContext, string style)
    {
        metroContext.Styles.Add(style);
        return metroContext;
    }
}