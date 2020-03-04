using CoreProject.DataAccess.Context;
using CoreProject.DataAccess.Repository;
using CoreProject.Models;
using CoreProject.Settings;
using Di;
using Xunit;

namespace DiTest.cs
{
    public class DiTest
    {
        [Fact]
        public void Test1()
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

        [Fact]
        public void Singletone_Object_Return_Only_One_Object() 
        {
            var container = new DIBuilder()
                .AddStatic(sp => new MongoSetting() { DatabaseName = "BERRIES", ConntectionString = "www.google.com" })
                .Build();

            var s1 = container.GetService<MongoSetting>();
            var s2 = container.GetService<MongoSetting>();
            Assert.True(object.ReferenceEquals(s1, s2));
        }

        [Fact]
        public void Singletone_Object_Return_New_One_Always()
        {
            var container = new DIBuilder()
                .AddTransient(sp => new MongoSetting() { DatabaseName = "BERRIES", ConntectionString = "www.google.com" })
                .Build();

            var s1 = container.GetService<MongoSetting>();
            var s2 = container.GetService<MongoSetting>();
            Assert.False(object.ReferenceEquals(s1, s2));
        }
    }
}
