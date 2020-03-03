using System;

namespace DI
{
    public class ServiceDescriptor
    {
        private object _syncObject = new object();

        public Type ImplementatinType { get; }

        private object _implObject;

        public object ImplementationInstance 
        {
            get
            {
                if (Lifetime == ServiceLifetime.Singleton)
                {
                    if (_implObject == null)
                    {
                        lock (_syncObject)
                        {
                            if (_implObject == null)
                            {
                                _implObject = ImplementationFactory?.Invoke();
                            }    
                        }
                    }

                    return _implObject;
                }
                else
                {
                    return ImplementationFactory?.Invoke();
                }
            }   
        }

        public ServiceLifetime Lifetime { get; }

        public Type ServiceType { get; }

        public Func<Abstractions.IServiceProvider, object> ImplementationFactory { get; }

        private Abstractions.IServiceProvider _serviceProvider;

        public ServiceDescriptor(
            Abstractions.IServiceProvider serviceProvider,
            Type implementatinType, 
            ServiceLifetime lifetime, 
            Type serviceType,
            Func<Abstractions.IServiceProvider, object> implementationFactory)
        {
            ImplementatinType = implementatinType;
            Lifetime = lifetime;
            ServiceType = serviceType;
            ImplementationFactory = implementationFactory;
            _serviceProvider = serviceProvider;
        }
    }
}
