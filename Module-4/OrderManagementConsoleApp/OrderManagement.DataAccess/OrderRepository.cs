using OrderManagement.DataAccess.Contract.Interfaces;
using OrderManagement.DataAccess.Contract.Models;
using OrderManagement.DataAccess.Properties;
using System.Data.Common;

namespace OrderManagement.DataAccess
{
    public class OrderRepository : AdoAbstractRepository<Order>, IOrderRepository
    {
        public OrderRepository(string connectionString, string nameProvider) :
            base(connectionString, nameProvider)
        {
            GetByIdSql = Resources.GetByIdSql;
            GetCollectionSql = Resources.GetCollectionSql;
            UpdateSql = Resources.UpdateSql;
            DeleteSql = Resources.DeleteSql;
            InsertSql = Resources.InsertSql;

            //... need to move code bellow in resources.
            GetByIdSql = "select  OrderID," +
                                "CustomerID," +
                                "EmployeeID," +
                                "OrderDate," +
                                "RequiredDate," +
                                "ShippedDate," +
                                "ShipVia," +
                                "Freight," +
                                "ShipName," +
                                "ShipAddress," +
                                "ShipCity, " +
                                "ShipRegion," +
                                "ShipPostalCode," +
                                "ShipCountry," +
                                "case " +
                                "    when OrderDate is null then 0    " +
                                "    when ShippedDate is null then 1  " +
                                "    else 2                           " +
                                "end as 'Status'                      " +
                                "from dbo.Orders as Orders " +
                                "where OrderId = @id;";

            UpdateSql = "" +
                            "UPDATE table_name" +
                            "SET " +
                            "   column1 = value1, " +
                            "   column2 = value2, " +
                            "WHERE OrderId=@id";

            DeleteSql = "DELETE FROM dbo.Orders WHERE OrderId=@id";

            GetCollectionSql = "select  OrderID," +
                                   "CustomerID," +
                                   "EmployeeID," +
                                   "OrderDate," +
                                   "RequiredDate," +
                                   "ShippedDate," +
                                   "ShipVia," +
                                   "Freight," +
                                   "ShipName," +
                                   "ShipAddress," +
                                   "ShipCity, " +
                                   "ShipRegion," +
                                   "ShipPostalCode," +
                                   "ShipCountry," +
                                   "case " +
                                   "    when OrderDate is null then 0    " +
                                   "    when ShippedDate is null then 1  " +
                                   "    else 2                           " +
                                   "end as 'Status'                      " +
                                   "from dbo.Orders as Orders;";
        }

        public int DeleteNotCompletedOrders()
        {
            int deletedRows = 0;
            //..//
            return deletedRows;
        }

        protected override Order FromReaderToObject(DbDataReader reader)
        {
            var order = new Order(
                orderDate: reader.SafeCastNullableDateTime(3),
                shippedDate: reader.SafeCastNullableDateTime(5),
                status: (OrderStatus)reader.GetInt32(14)
            );

            order.Id = reader.SafeCastInt32(0);
            order.CustomerId = reader.SafeCastString(1);
            order.EmployeeId = reader.SafeCastNullableInt32(2);
            order.RequiredDate = reader.SafeCastNullableDateTime(4);
            order.ShipVia = reader.SafeCastNullableInt32(6);
            order.Freight = reader.SafeCastNullableDecimal(7);
            order.ShipName = reader.SafeCastString(8);
            order.ShipAddress = reader.SafeCastString(9);
            order.ShipCity = reader.SafeCastString(10);
            order.ShipRegion = reader.SafeCastString(11);
            order.ShipPostalCode = reader.SafeCastString(12);
            order.ShipCountry = reader.SafeCastString(13);
            return order;
        }
    }
}