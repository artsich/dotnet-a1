using CoreProject.DataAccess.Context;
using CoreProject.DataAccess.Repository;
using CoreProject.Models;
using CoreProject.Settings;
using Di;
using DiTest.TestProject.Services;
using FluentAssertions;
using Xunit;

namespace DiTest.cs
{
    public class DiTest
    {
        [Fact]
        public void Add_Singletone_Object_Return_Only_One_Object()
        {
            var container = new DIBuilder()
                .AddStatic(sp => new MongoSetting() { DatabaseName = "BERRIES", ConntectionString = "www.google.com" })
                .Build();

            var s1 = container.GetService<MongoSetting>();
            var s2 = container.GetService<MongoSetting>();
            ReferenceEquals(s1, s2).Should().BeTrue();
        }

        [Fact]
        public void Add_Transient_Object_Return_New_Object_Always()
        {
            var container = new DIBuilder()
                .AddTransient(sp => new MongoSetting() { DatabaseName = "BERRIES", ConntectionString = "www.google.com" })
                .Build();

            var s1 = container.GetService<MongoSetting>();
            var s2 = container.GetService<MongoSetting>();
            ReferenceEquals(s1, s2).Should().BeFalse();
        }

        [Fact]
        public void Injection_To_Constructor_With_Arguments()
        {
            var container = new DIBuilder()
                .AddStatic(sp => new MongoSetting() { DatabaseName = "BERRIES", ConntectionString = "www.google.com" })
                .AddTransient<IContext, MongoContext>()
                .Build();
            
            var context = container.GetService<IContext>();
            context.Should().NotBeNull();
            context.Should().BeOfType<MongoContext>();
        }

        [Fact]
        public void Injection_Test_With_Implementation_Factory_Return_Object_By_Class_Name()
        {
            var container = new DIBuilder()
                .AddTransient(sp => new MongoSetting() { DatabaseName = "BERRIES", ConntectionString = "www.google.com" })
                .Build();

            var setting = container.GetService<MongoSetting>();
            setting.Should().NotBeNull();
            setting.Should().BeOfType<MongoSetting>();

            setting.DatabaseName.Should().BeEquivalentTo("BERRIES");
            setting.ConntectionString.Should().BeEquivalentTo("www.google.com");
        }

        [Fact]
        public void Inject_To_Properties_Return_Object_With_Injected_Properties()
        {
            var container = new DIBuilder()
                .AddStatic(sp => new MongoSetting() { DatabaseName = "BERRIES", ConntectionString = "www.google.com" })
                .AddTransient<IContext, MongoContext>()
                .AddTransient<IRepository<User>, UserRepository>()
                .AddTransient<IRepository<Product>, ProductRepository>()
                .AddTransient<ShopService>()
                .Build();

            var shopService = container.GetService<ShopService>();
            shopService.Should().NotBeNull();
            shopService.UserRepository.Should().BeOfType<UserRepository>();
            shopService.ProductRepository.Should().BeOfType<ProductRepository>();
        }
    }
}
