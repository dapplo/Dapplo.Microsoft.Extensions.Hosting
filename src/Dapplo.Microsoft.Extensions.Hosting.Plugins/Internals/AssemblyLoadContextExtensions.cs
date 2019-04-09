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

using System.Reflection;
using System.Runtime.Loader;

namespace Dapplo.Microsoft.Extensions.Hosting.Plugins.Internals
{
    /// <summary>
    /// AssemblyLoadContext extensions
    /// </summary>
    public static class AssemblyLoadContextExtensions
    {
        /// <summary>
        /// Try to get an assembly from the specified AssemblyLoadContext
        /// </summary>
        /// <param name="assemblyLoadContext">AssemblyLoadContext</param>
        /// <param name="assemblyName">AssemblyName to look for</param>
        /// <param name="foundAssembly">Assembly out</param>
        /// <returns>bool</returns>
        public static bool TryGetAssembly(this AssemblyLoadContext assemblyLoadContext, AssemblyName assemblyName, out Assembly foundAssembly)
        {
            foreach (var assembly in assemblyLoadContext.Assemblies)
            {
                if (!assembly.GetName().Name.Equals(assemblyName.Name))
                {
                    continue;
                }
                foundAssembly = assembly;
                return true;
            }
            foundAssembly = null;
            return false;
        }
    }
}