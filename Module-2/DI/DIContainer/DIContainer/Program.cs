using CoreProject.DataAccess.Context;
using CoreProject.DataAccess.Repository;
using CoreProject.Models;
using CoreProject.Settings;
using Di;

namespace DIContainer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = new DIBuilder()
                .AddStatic(sp => new MongoSetting() { DatabaseName = "BERRIES", ConntectionString = "www.google.com" })
                .AddTransient<IContext, MongoContext>()
                .AddTransient<IRepository<User>, UserRepository>()
                .AddTransient<IRepository<Product>, ProductRepository>()
                .Build();

            var setting = container.GetService<MongoSetting>();
            var context = container.GetService<IContext>();
            var repUser = container.GetService<IRepository<User>>();
            var repProduct = container.GetService<IRepository<Product>>();
        }
    }
}
