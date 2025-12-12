// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Dapplo.Hosting.Sample.AvaloniaDemo;

/// <summary>
/// A simple ViewLocator to locate views based on ViewModel type names.
/// This is optional and can be used if you follow MVVM pattern.
/// </summary>
public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
            return null;

        var name = data.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is { };
    }
}
