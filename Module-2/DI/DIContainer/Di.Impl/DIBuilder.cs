using System;
using System.Collections.Generic;
using DI.Abstractions;

namespace DI
{
    public class DIBuilder : IContainerBuilder
    {
        private Dictionary<Type, ServiceDescriptor> _descriptorMap;

        public IContainerBuilder AddStatic<TService>()
        {
            var serviceType = typeof(TService);
            if (!_descriptorMap.TryGetValue(serviceType, out _))
            {
                _descriptorMap[serviceType] = new ServiceDescriptor(serviceType, ServiceLifetime.Singleton, serviceType, null);
            }
            else
            {
                throw new NotImplementedException("TODO: CHECK IT EXCEPTION");
            }

            return this;
        }

        public IContainerBuilder AddStatic<TService, TImplementation>() 
            where TImplementation : class, TService
        {
            var serviceType = typeof(TService);
            if (!_descriptorMap.TryGetValue(serviceType, out _))
            {
                _descriptorMap[serviceType] = new ServiceDescriptor(serviceType, ServiceLifetime.Singleton, typeof(TImplementation), null);
            }
            else
            {
                throw new NotImplementedException("TODO: CHECK IT EXCEPTION");
            }

            return this;
        }

        public IContainerBuilder AddStatic<TService, TImplementation>(Func<Abstractions.IServiceProvider, TImplementation> implementationFactory) 
            where TImplementation : class, TService
        {
            var serviceType = typeof(TService);
            if (!_descriptorMap.TryGetValue(serviceType, out _))
            {
                _descriptorMap[serviceType] = new ServiceDescriptor(serviceType, ServiceLifetime.Singleton, typeof(TImplementation), implementationFactory);
            }
            else
            {
                throw new NotImplementedException("TODO: CHECK IT EXCEPTION");
            }

            return this;
        }

        public IContainerBuilder AddStatic<TService>(Func<Abstractions.IServiceProvider, TService> implementationFactory) where TService : class
        {
            var serviceType = typeof(TService);
            if (!_descriptorMap.TryGetValue(serviceType, out _))
            {
                _descriptorMap[serviceType] = new ServiceDescriptor(serviceType, ServiceLifetime.Singleton, serviceType, implementationFactory);
            }
            else
            {
                throw new NotImplementedException("TODO: CHECK IT EXCEPTION");
            }

            return this;
        }

        public IContainerBuilder AddTransient<TService>() 
            where TService : class
        {
            var serviceType = typeof(TService);
            if (!_descriptorMap.TryGetValue(serviceType, out _))
            {
                _descriptorMap[serviceType] = new ServiceDescriptor(serviceType, ServiceLifetime.Singleton, serviceType, null);
            }
            else
            {
                throw new NotImplementedException("TODO: CHECK IT EXCEPTION");
            }

            return this;
        }

        public IContainerBuilder AddTransient<TService, TImplementation>() 
            where TImplementation : class, TService
        {
            var serviceType = typeof(TService);
            if (!_descriptorMap.TryGetValue(serviceType, out _))
            {
                _descriptorMap[serviceType] = new ServiceDescriptor(serviceType, ServiceLifetime.Singleton, typeof(TImplementation), null);
            }
            else
            {
                throw new NotImplementedException("TODO: CHECK IT EXCEPTION");
            }

            return this;
        }

        public IContainerBuilder AddTransient<TService, TImplementation>(Func<Abstractions.IServiceProvider, TImplementation> implementationFactory) 
            where TImplementation : class, TService
        {
            var serviceType = typeof(TService);
            if (!_descriptorMap.TryGetValue(serviceType, out _))
            {
                _descriptorMap[serviceType] = new ServiceDescriptor(serviceType, ServiceLifetime.Singleton, typeof(TImplementation), implementationFactory);
            }
            else
            {
                throw new NotImplementedException("TODO: CHECK IT EXCEPTION");
            }

            return this;
        }

        public Abstractions.IServiceProvider Build()
        {
            return new DIContainer();
        }
    }
}
