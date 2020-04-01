using OrderManagement.DataAccess.Repositories;
using System;

namespace Sandbox
{

    class Program
    {
        const string ConnectionString = "Server=(localdb)\\ProjectsV13;Database=Northwind;Integrated Security=True";
        const string ProviderName = "System.Data.SqlClient";

        static void Main(string[] args)
        {

            var repo = new ProductRepository(ConnectionString, ProviderName);
            //var res = repo.GetAll();
            //int rows = repo.MoveProductToAnotherCategory(1, 2);

            var repoOrder = new OrderDetailRepo(ConnectionString, ProviderName);
            //var isChanged = repoOrder.ReplaceProduct(11008, 33, 67);

            var repo1 = new EmployeeRepo(ConnectionString, ProviderName);
            var emp = repo1.Get(15);
            emp.FirstName = "Artrsiom";
            repo1.Update(emp);

            //var statList = repo1.GetStatByRegions();
            //var stattList = repo1.GetEmplsWithShips();
            //var sss = repo1.GetEmplManagedTerritories();

            Console.ReadKey();
        }
    }
}
