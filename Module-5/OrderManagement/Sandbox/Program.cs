using OrderManagement.DataAccess.Models.Db;
using OrderManagement.DataAccess.Repositories;
using OrderManagement.Services;
using System;
using System.Collections.Generic;

namespace Sandbox
{
    class Program
    {
        const string ConnectionString = "Server=(localdb)\\ProjectsV13;Database=Northwind;Integrated Security=True";
        const string ProviderName = "System.Data.SqlClient";

        static void Main(string[] args)
        {
            TerritoryRepoTest();
            Console.ReadKey();
        }

        static void EmplServiceTest()
        {
            var emplRepo = new EmployeeRepo(ConnectionString, ProviderName);
            var terrRepo = new TerritoryRepo(ConnectionString, ProviderName);
            var terrs = new List<Territory>
            {
                new Territory() { RegionId=1, TerritoryDescription = "newnewnewnew", TerritoryID = "covid-19" },
                new Territory() { RegionId=1, TerritoryDescription = "dsadasdad", TerritoryID = "0001" },
                new Territory() { RegionId=1, TerritoryDescription = "dsadasdad", TerritoryID = "0002" },
                new Territory() { RegionId=1, TerritoryDescription = "blblblbllb", TerritoryID = "000001" },
            };

            var newEmpl = new Employee("Merzyl", "asus")
            {
                Territories = terrs
            };

            var service = new EmplService(emplRepo, terrRepo);
            service.Insert(newEmpl);
        }

        static void TerritoryRepoTest()
        {
            var repo = new TerritoryRepo(ConnectionString, ProviderName);
            var terrs = new List<Territory>
            {
                new Territory() { RegionId=1, TerritoryDescription = "dsadasdad", TerritoryID = "0001" },
                new Territory() { RegionId=1, TerritoryDescription = "cs.go", TerritoryID = "0006" },
                new Territory() { RegionId=1, TerritoryDescription = "blblblbllb", TerritoryID = "000001" },
            };
            var affected = repo.TryInsertMany(terrs);
            Console.Write("Affected: {0}", affected);
        }

        static void ProductRepoTest()
        {
            var repo = new ProductRepository(ConnectionString, ProviderName);
            var res = repo.GetAll();
            int rows = repo.MoveProductToAnotherCategory(1, 2);
            var pr1 = repo.Get(1);
            repo.Insert(pr1);
        }

        static void OrderRepoTest()
        {
            var repoOrder = new OrderDetailRepo(ConnectionString, ProviderName);
            var isChanged = repoOrder.ReplaceProduct(11008, 33, 67);
        }

        static void EmployeeRepoTest()
        {
            var repo1 = new EmployeeRepo(ConnectionString, ProviderName);
            var emp = repo1.Get(15);
            emp.FirstName = "Artrsiom";
            repo1.Update(emp);
            var statList = repo1.GetStatByRegions();
            var stattList = repo1.GetEmplsWithShips();
            var sss = repo1.GetEmplManagedTerritories();
        }
    }
}
