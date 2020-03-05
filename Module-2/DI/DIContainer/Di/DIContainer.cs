using Di.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Di
{
    public class DIContainer : Abstractions.IServiceProvider, IContainerBuilder
    {
        private IDictionary<Assembly, Type[]> _assemblyTypesCache;
        private IDictionary<Type, AbstractServiceDescriptor> _descriptionMap;

        public DIContainer()
        {
            _assemblyTypesCache = new Dictionary<Assembly, Type[]>();
            _descriptionMap = new Dictionary<Type, AbstractServiceDescriptor>();
        }

        public TService GetService<TService>()
            where TService : class
        {
            return GetService(typeof(TService)) as TService;
        }

        public object GetService(Type type) 
        {
            if (_descriptionMap.TryGetValue(type, out var resultService))
            {
                return resultService.ImplementationInstance;
            }
            else
            {
                return AddTypeToMap(type);
            }

            throw new Exception("Can't find this type");
        }

        private object AddTypeToMap(Type type)
        {
            if (type.IsInterface || type.IsAbstract)
            {
                var curAssem = Assembly.GetAssembly(type);

                if (!_assemblyTypesCache.ContainsKey(curAssem))
                {
                    _assemblyTypesCache[curAssem] = curAssem.GetTypes();
                }

                var inheritTypes = _assemblyTypesCache[curAssem].Where(p => !p.Equals(type) && type.IsAssignableFrom(p)).ToList();

                if(inheritTypes.Count() == 1)
                {
                    var description = new TransientServiceDescriptor(
                        this,
                        type,
                        inheritTypes.First(),
                        null);
                    _descriptionMap[type] = description;
                }
                else
                {
                    throw new Exception($"Type {type} has more then one child classes");
                }
            }
            else
            {
                var description = new TransientServiceDescriptor(
                    this,
                    type,
                    type,
                    null);
                _descriptionMap[type] = description;
            }

            return _descriptionMap[type].ImplementationInstance;
        }

        public IContainerBuilder AddStatic<TService>()
        {
            var serviceType = typeof(TService);
            _descriptionMap[serviceType] = new StaticServiceDescriptor(this, serviceType, serviceType, null);
            return this;
        }

        public IContainerBuilder AddStatic<TService, TImplementation>()
            where TImplementation : class, TService
        {
            var serviceType = typeof(TService);
            _descriptionMap[serviceType] = new StaticServiceDescriptor(this, serviceType, typeof(TImplementation), null);
            return this;
        }

        public IContainerBuilder AddStatic<TService, TImplementation>(Func<Abstractions.IServiceProvider, TImplementation> implementationFactory)
            where TImplementation : class, TService
        {
            var serviceType = typeof(TService);
            _descriptionMap[serviceType] = new StaticServiceDescriptor(this, serviceType, typeof(TImplementation), implementationFactory);
            return this;
        }

        public IContainerBuilder AddStatic<TService>(Func<Abstractions.IServiceProvider, TService> implementationFactory) where TService : class
        {
            var serviceType = typeof(TService);
            _descriptionMap[serviceType] = new StaticServiceDescriptor(this, serviceType, serviceType, implementationFactory);
            return this;
        }

        public IContainerBuilder AddTransient<TService>()
            where TService : class
        {
            var serviceType = typeof(TService);
            _descriptionMap[serviceType] = new TransientServiceDescriptor(this, serviceType, serviceType, null);
            return this;
        }

        public IContainerBuilder AddTransient<TService, TImplementation>()
            where TImplementation : class, TService
        {
            var serviceType = typeof(TService);
            _descriptionMap[serviceType] = new TransientServiceDescriptor(this, serviceType, typeof(TImplementation), null);
            return this;
        }

        public IContainerBuilder AddTransient<TService, TImplementation>(Func<Abstractions.IServiceProvider, TImplementation> implementationFactory)
            where TImplementation : class, TService
        {
            var serviceType = typeof(TService);
            _descriptionMap[serviceType] = new TransientServiceDescriptor(this, serviceType, typeof(TImplementation), implementationFactory);
            return this;
        }

        public IContainerBuilder AddTransient<TService>(Func<Abstractions.IServiceProvider, TService> implementationFactory) where TService : class
        {
            var serviceType = typeof(TService);
            _descriptionMap[serviceType] = new TransientServiceDescriptor(this, serviceType, serviceType, implementationFactory);
            return this;
        }

        public Abstractions.IServiceProvider Build()
        {
            return this;
        }
    }
}
