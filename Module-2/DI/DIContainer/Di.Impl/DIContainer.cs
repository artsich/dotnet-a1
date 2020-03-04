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

        public TService GetSertice<TService>()
            where TService : class
        {
            var type = typeof(TService);
            if (_descriptionMap.TryGetValue(type, out var resultService))
            {
                return resultService.ImplementationInstance as TService;
            }

            throw new Exception("TOODSODOSAPODJKSADJSALKDJLSKJd;lkj");
        }

        public TService GetSertice<TService>(TService type) where TService : class
        {
            return GetSertice<TService>();
        }
    }
}
