using System;

namespace Di
{
    public class StaticServiceDescriptor : AbstractServiceDescriptor
    {
        private object _syncObject = new object();

        private object _implObject;

        public override object ImplementationInstance
        {
            get
            {
                if (_implObject == null)
                {
                    lock (_syncObject)
                    {
                        if (_implObject == null)
                        {
                            if (ImplementationFactory == null)
                            {
                                _implObject = CreateInstance();
                            }
                            else
                            {
                                _implObject = ImplementationFactory(_serviceProvider);
                            }
                        }
                    }
                }
                return _implObject;
            }
        }

        public StaticServiceDescriptor(Abstractions.IServiceProvider serviceProvider,
                Type serviceType,
                Type implementatinType,
                Func<Abstractions.IServiceProvider, object> implementationFactory
        ): base(serviceProvider,
                serviceType,
                implementatinType,
                implementationFactory)
        {
        }
    }
}
