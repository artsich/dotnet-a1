﻿using System;
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
                try
                {
                    ctorParams[i] = _serviceProvider.GetService(ctorArgs[i].ParameterType);
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
            var type = ImplementationType;
            var injectingProperties = type.GetProperties().Where(pr => pr.GetCustomAttribute<ImportAttribute>() != null);
            foreach (var prop in injectingProperties)
            {
                prop.SetValue(obj, _serviceProvider.GetService(prop.DeclaringType));
            }
        }
    }
}