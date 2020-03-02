namespace DI.Abstractions
{
    public interface IServiceProvider
    {
        TService GetSertice<TService>() where TService : class;
    }
}
