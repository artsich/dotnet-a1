using System;

namespace Di.Abstractions
{
    public interface IContainerBuilder
    {
        IContainerBuilder AddTransient<TService>()
            where TService : class;

        IContainerBuilder AddTransient<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class;

        IContainerBuilder AddTransient<TService, TImplementation>()
            where TImplementation : class, TService;

        IContainerBuilder AddTransient<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
            where TImplementation : class, TService;

        IContainerBuilder AddStatic<TService>();

        IContainerBuilder AddStatic<TService>(Func<IServiceProvider, TService> implementationFactory) 
            where TService : class;

        IContainerBuilder AddStatic<TService, TImplementation>()
            where TImplementation : class, TService;

        IContainerBuilder AddStatic<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
            where TImplementation : class, TService;

        IServiceProvider Build();
    }
}
