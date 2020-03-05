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
                    return ImplementationFactory(_serviceProvider);
                }
                else
                {
                    return CreateInstance();
                }
            }
        }

        public TransientServiceDescriptor(Abstractions.IServiceProvider serviceProvider,
                Type serviceType,
                Type implementatinType,
                Func<Abstractions.IServiceProvider, object> implementationFactory
        ) : base(serviceProvider,
                serviceType,
                implementatinType,
                implementationFactory)
        {
        }
    }
}