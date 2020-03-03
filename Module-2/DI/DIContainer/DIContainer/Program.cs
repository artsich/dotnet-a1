using CoreProject.DataAccess.Context;
using CoreProject.DataAccess.Repository;
using CoreProject.Models;
using CoreProject.Settings;
using DI;

namespace DIContainer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new DIBuilder().
                AddStatic(sp => new MongoSetting() { DatabaseName = "", ConntectionString = "" }).
                AddTransient<IContext, MongoContext>().
                AddTransient<IRepository<User>, UserRepository>().
                AddTransient<IRepository<Product>, ProductRepository>();
        }
    }
}
