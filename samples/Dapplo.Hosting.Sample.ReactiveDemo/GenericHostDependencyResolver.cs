using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Splat;

namespace Dapplo.Hosting.Sample.ReactiveDemo
{
    public class GenericHostDependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public GenericHostDependencyResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType, string contract = null)
        {
            return _serviceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType, string contract = null)
        {
            return _serviceProvider.GetServices(serviceType);
        }

        public void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            throw new NotImplementedException();
        }

        public void UnregisterCurrent(Type serviceType, string contract = null)
        {
            throw new NotImplementedException();
        }

        public void UnregisterAll(Type serviceType, string contract = null)
        {
            throw new NotImplementedException();
        }

        public IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}
