// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Splat;

namespace Dapplo.Microsoft.Extensions.Hosting.ReactiveUI
{
    /// <summary>
    /// This is the dependency resolver which is used from ReactiveUI
    /// </summary>
    public class GenericHostDependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// The DI constructor
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider</param>
        public GenericHostDependencyResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Locator.SetLocator(this); 
        }

        /// <inheritdoc />
        public object GetService(Type serviceType, string contract = null)
        {
            return _serviceProvider.GetService(serviceType);
        }

        /// <inheritdoc />
        public IEnumerable<object> GetServices(Type serviceType, string contract = null)
        {
            return _serviceProvider.GetServices(serviceType);
        }

        /// <inheritdoc />
        public void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void UnregisterCurrent(Type serviceType, string contract = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void UnregisterAll(Type serviceType, string contract = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}
