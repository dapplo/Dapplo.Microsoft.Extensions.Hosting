//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2019 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Microsoft.Extensions.Hosting
// 
//  Dapplo.Microsoft.Extensions.Hosting is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Microsoft.Extensions.Hosting is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Microsoft.Extensions.Hosting. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

using System;

namespace Dapplo.Microsoft.Extensions.Hosting.Plugins
{
    /// <summary>
    ///     Use this attribute to specify the order for loading plug-ins
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class PluginOrderAttribute : Attribute
    {
        /// <summary>
        /// Default value
        /// </summary>
        public PluginOrderAttribute()
        {
        }

        /// <summary>
        /// Specify the order of the plug-in initialization
        /// </summary>
        public PluginOrderAttribute(int order)
        {
            Order = order;
        }

        /// <summary>
        /// Specify the order by using enums
        /// </summary>
        /// <param name="enumValue">object which can be converted to an int</param>
        public PluginOrderAttribute(object enumValue) : this(Convert.ToInt32(enumValue))
        {
        }

        /// <summary>
        /// Order to initialize the plug-in
        /// </summary>
        public int Order { get; }
    }
}
