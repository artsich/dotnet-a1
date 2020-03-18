using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement
{
    public class Program
    {
        static void Main(string[] args)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder();
            connectionStringBuilder.DataSource = "(local)";
            connectionStringBuilder.InitialCatalog = "Northwind";
            connectionStringBuilder.IntegratedSecurity = true;

            var connString = "Server=(localdb)\\ProjectsV13;Database=Northwind;Integrated Security=True";

            var rep = new DataAccess.OrderRepository(connString, "System.Data.SqlClient");

            var collection = rep.GetCollection();
            Console.ReadKey();
        }
    }
}
