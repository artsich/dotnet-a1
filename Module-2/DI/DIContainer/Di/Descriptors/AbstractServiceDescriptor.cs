using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Di
{
    public abstract class AbstractServiceDescriptor
    {
        public Type ImplementationType { get; }

        public virtual object ImplementationInstance { get; }

        public Type ServiceType { get; }

        public Func<Abstractions.IServiceProvider, object> ImplementationFactory { get; }

        protected Abstractions.IServiceProvider _serviceProvider;

        public AbstractServiceDescriptor(
            Abstractions.IServiceProvider serviceProvider,
            Type serviceType,
            Type implementatinType,
            Func<Abstractions.IServiceProvider, object> implementationFactory)
        {
            ImplementationType = implementatinType;
            ServiceType = serviceType;
            ImplementationFactory = implementationFactory;
            _serviceProvider = serviceProvider;
        }

        protected object CreateInstance()
        {
            var obj = InjectToCtor();
            InjectToProperty(obj);
            return obj;
        }

        private object InjectToCtor()
        {
            var type = ImplementationType;
            object[] ctorParams = null;
            var ctors = GetPrimaryCtor(type);
            var ctor = ctors.FirstOrDefault(ct => ResolveDependency(ct, out ctorParams));
            if (ctor != null)
            {
                return ctor.Invoke(ctorParams);
            }

            throw new Exception("Can't find corresponding constructor for this type");
        }

        private ICollection<ConstructorInfo> GetPrimaryCtor(Type type)
        {
            var ctors = type.GetConstructors();
            var sorted = ctors.OrderBy(x => x.GetParameters().Length).ToList();
            return sorted;
        }

        private bool ResolveDependency(ConstructorInfo ctorInfo, out object[] ctorParams)
        {
            var ctorArgs = ctorInfo.GetParameters();
            ctorParams = new object[ctorArgs.Length];
            for (int i = 0; i < ctorParams.Length; ++i)
            {
                var service = _serviceProvider.GetService(ctorArgs[i].ParameterType);
                if (service != null)
                {
                    ctorParams[i] = service;
                }
                else
                {
                    throw new Exception($"Can't resolve dependency for type: {ctorArgs[i].ParameterType}");
                }
            }

            return true;
        }

        private void InjectToProperty(object obj)
        {
            var type = ImplementationType;
            var props = type.GetProperties();
            var injectingProperties = props.Where(pr => pr.GetCustomAttribute<InjectAttribute>() != null);

            foreach (var prop in injectingProperties)
            {
                prop.SetValue(obj, _serviceProvider.GetService(prop.PropertyType));
            }
        }
    }
}
