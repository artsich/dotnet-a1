using System;

using System.Collections.Generic;

namespace Di
{
    public class DIContainer : Abstractions.IServiceProvider
    {
        private IDictionary<Type, AbstractServiceDescriptor> _descriptionMap;

        public DIContainer(Dictionary<Type, AbstractServiceDescriptor> descriptionMap)
        {
            _descriptionMap = descriptionMap;
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

            throw new Exception("TOODSODOSAPODJKSADJSALKDJLSKJd;lkj");
        }
    }
}
