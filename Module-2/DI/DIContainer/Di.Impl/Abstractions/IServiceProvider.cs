namespace Di.Abstractions
{
    public interface IServiceProvider
    {
        TService GetSertice<TService>() 
            where TService : class;

        TService GetSertice<TService>(TService type) 
            where TService : class;
    }
}
