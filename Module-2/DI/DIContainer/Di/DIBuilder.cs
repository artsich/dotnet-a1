using System;
using System.Collections.Generic;
using Di.Abstractions;

namespace Di
{
    public class DIBuilder : IContainerBuilder
    {
        private DIContainer _serviceProvider;

        private Dictionary<Type, AbstractServiceDescriptor> _descriptorMap;

        public DIBuilder()
        {
            _descriptorMap = new Dictionary<Type, AbstractServiceDescriptor>();
            _serviceProvider = new DIContainer(_descriptorMap);
        }

        public IContainerBuilder AddStatic<TService>()
        {
            var serviceType = typeof(TService);
            _descriptorMap[serviceType] = new StaticServiceDescriptor(_serviceProvider, serviceType, serviceType, null);
            return this;
        }

        public IContainerBuilder AddStatic<TService, TImplementation>() 
            where TImplementation : class, TService
        {
            var serviceType = typeof(TService);
            _descriptorMap[serviceType] = new StaticServiceDescriptor(_serviceProvider, serviceType, typeof(TImplementation), null);
            return this;
        }

        public IContainerBuilder AddStatic<TService, TImplementation>(Func<Abstractions.IServiceProvider, TImplementation> implementationFactory) 
            where TImplementation : class, TService
        {
            var serviceType = typeof(TService);
            _descriptorMap[serviceType] = new StaticServiceDescriptor(_serviceProvider, serviceType, typeof(TImplementation), implementationFactory);
            return this;
        }

        public IContainerBuilder AddStatic<TService>(Func<Abstractions.IServiceProvider, TService> implementationFactory) where TService : class
        {
            var serviceType = typeof(TService);
            _descriptorMap[serviceType] = new StaticServiceDescriptor(_serviceProvider, serviceType, serviceType, implementationFactory);
            return this;
        }

        public IContainerBuilder AddTransient<TService>() 
            where TService : class
        {
            var serviceType = typeof(TService);
            _descriptorMap[serviceType] = new TransientServiceDescriptor(_serviceProvider, serviceType, serviceType, null);
            return this;
        }

        public IContainerBuilder AddTransient<TService, TImplementation>() 
            where TImplementation : class, TService
        {
            var serviceType = typeof(TService);
            _descriptorMap[serviceType] = new TransientServiceDescriptor(_serviceProvider, serviceType, typeof(TImplementation), null);
            return this;
        }

        public IContainerBuilder AddTransient<TService, TImplementation>(Func<Abstractions.IServiceProvider, TImplementation> implementationFactory) 
            where TImplementation : class, TService
        {
            var serviceType = typeof(TService);
            _descriptorMap[serviceType] = new TransientServiceDescriptor(_serviceProvider, serviceType, typeof(TImplementation), implementationFactory);
            return this;
        }

        public IContainerBuilder AddTransient<TService>(Func<Abstractions.IServiceProvider, TService> implementationFactory) where TService : class
        {
            var serviceType = typeof(TService);
            _descriptorMap[serviceType] = new TransientServiceDescriptor(_serviceProvider, serviceType, serviceType, implementationFactory);
            return this;
        }

        public Abstractions.IServiceProvider Build()
        {
            var result = _serviceProvider;
            //TODO: Need to think about it.
            _descriptorMap = new Dictionary<Type, AbstractServiceDescriptor>();
            _serviceProvider = new DIContainer(_descriptorMap);
            return result;
        }
    }
}
