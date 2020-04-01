using Dapper;
using OrderManagement.DataAccess.Contract.Models;
using OrderManagement.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{

    class Program
    {
        const string ConnectionString = "Server=(localdb)\\ProjectsV13;Database=Northwind;Integrated Security=True";

        static void Main(string[] args)
        {
            var repo = new ProductRepository(ConnectionString, "System.Data.SqlClient");
            var res = repo.GetAll();
            int rows = repo.MoveProductToAnotherCategory(1, 2);

            var repoOrder = new OrderDetailRepo(ConnectionString, "System.Data.SqlClient");
            var isChanged = repoOrder.ReplaceProduct(11008, 33, 67);

            var repo1 = new EmployeeRepo(ConnectionString, "System.Data.SqlClient");
            var statList = repo1.GetStatByRegions();
            var stattList = repo1.GetEmplsWithShips();
            var sss = repo1.GetEmplManagedTerritories();

            Console.ReadKey();
        }
    }
}
