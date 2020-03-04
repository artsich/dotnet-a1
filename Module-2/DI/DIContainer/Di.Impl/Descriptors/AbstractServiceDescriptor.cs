using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Di
{
    public abstract class AbstractServiceDescriptor
    {
        public Type ImplementatinType { get; }

        public virtual object ImplementationInstance { get; }

        public Type ServiceType { get; }

        public Func<Abstractions.IServiceProvider, object> ImplementationFactory { get; }

        protected Abstractions.IServiceProvider _serviceProvider;

        public AbstractServiceDescriptor(
            Abstractions.IServiceProvider serviceProvider,
            Type implementatinType,
            Type serviceType,
            Func<Abstractions.IServiceProvider, object> implementationFactory)
        {
            ImplementatinType = implementatinType;
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
            var type = ImplementatinType;
            object[] ctorParams = null;
            var ctor = GetPrimaryCtor(type).FirstOrDefault(ct => ResolveDependency(ct, out ctorParams));
            if (ctor != null)
            {
                return ctor.Invoke(ctorParams);
            }

            throw new Exception("Can't find corresponding constructor for this type");
        }

        private IEnumerable<ConstructorInfo> GetPrimaryCtor(Type type) => 
            type.GetConstructors(BindingFlags.Public).OrderBy(x => x.GetParameters().Length);

        private bool ResolveDependency(ConstructorInfo ctorInfo, out object[] ctorParams)
        {
            var ctorArgs = ctorInfo.GetParameters();
            ctorParams = new object[ctorArgs.Length];
            for (int i = 0; i < ctorParams.Length; ++i)
            {
                try
                {
                    ctorParams[i] = _serviceProvider.GetSertice(ctorArgs[i].ParameterType);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        private void InjectToProperty(object obj)
        {
            var type = ImplementatinType;
            var injectingProperties = type.GetProperties().Where(pr => pr.GetCustomAttribute<ImportAttribute>() != null);
            foreach (var prop in injectingProperties)
            {
                prop.SetValue(obj, _serviceProvider.GetSertice(prop.DeclaringType));
            }
        }
    }
}
