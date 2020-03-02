using DI;

namespace DIContainer
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new DIBuilder().
                AsStatic<Setting>();
        }
    }
}
