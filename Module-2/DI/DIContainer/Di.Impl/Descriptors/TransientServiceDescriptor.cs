using System;

namespace Di
{
    public class TransientServiceDescriptor : AbstractServiceDescriptor
    {
        public override object ImplementationInstance
        {
            get
            {
                if (ImplementationFactory != null)
                {
                    return ImplementationFactory?.Invoke(_serviceProvider);
                }
                else
                {
                    return CreateInstance();
                }
            }
        }

        public TransientServiceDescriptor(Abstractions.IServiceProvider serviceProvider,
                Type implementatinType,
                Type serviceType,
                Func<Abstractions.IServiceProvider, object> implementationFactory
        ) : base(serviceProvider,
                implementatinType,
                serviceType,
                implementationFactory)
        {
        }
    }
}