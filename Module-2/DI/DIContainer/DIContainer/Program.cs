using CoreProject.DataAccess.Context;
using CoreProject.DataAccess.Repository;
using CoreProject.Models;
using CoreProject.Settings;
using DI;

namespace DIContainer
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new DIBuilder().
                AddStatic<MongoSetting>(sp => new MongoSetting() { DatabaseName = "", ConntectionString = "" }).
                AddTransient<IContext, MongoContext>().
                AddTransient<IRepository<User>, UserRepository>().
                AddTransient<IRepository<Product>, ProductRepository>();
        }
    }
}
