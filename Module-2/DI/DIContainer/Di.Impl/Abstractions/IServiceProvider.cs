using System;

namespace Di.Abstractions
{
    public interface IServiceProvider
    {
        TService GetService<TService>()
            where TService : class;

        object GetService(Type type);
    }
}
