using OrderManagement.DataAccess.Contract.Models;
using OrderManagement.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement
{

    /*
     * todo: check update method OrderService
     */

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

            var or = new Order();
            rep.Insert(or);
            rep.Delete(or.Id);

            return;

            //var service = new OrderService(rep);
            //var order = service.GetOrder(10250);
            //var res = service.GetOrders();
            //var rew = service.GetById(10248);
            //return;

            var t = new DataAccess.OrderDetailRepository(connString, "System.Data.SqlClient");
            
            t.InsertDetailsInOrder(10248, new List<OrderDetail>()
            {
                new OrderDetail()
                {
                    ProductId = 51,
                    Quantity = 12,
                    UnitPrice = 14,
                    Discount = 0
                },
                new OrderDetail()
                {
                    ProductId = 14,
                    Quantity = 12,
                    UnitPrice = 14,
                    Discount = 0
                }
            });

            Console.ReadKey();
        }
    }
}