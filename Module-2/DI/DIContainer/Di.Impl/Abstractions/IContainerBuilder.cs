using System;

namespace DI.Abstractions
{
    public interface IContainerBuilder
    {
        IContainerBuilder AddTransient<TService>();

        IContainerBuilder AddTransient<TService, TImplementation>()
            where TImplementation : class, TService;

        IContainerBuilder AddTransient<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
            where TImplementation : class, TService;

        IContainerBuilder AddStatic<TService>();
        
        IContainerBuilder AddStatic<TService, TImplementation>()
            where TImplementation : class, TService;

        IContainerBuilder AddStatic<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
            where TImplementation : class, TService;

        IServiceProvider Build();
    }
}
