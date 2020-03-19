using OrderManagement.DataAccess.Contract;
using OrderManagement.DataAccess.Contract.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace OrderManagement.DataAccess
{

    public class OrderRepository : IRepository<Order>
    {
        private readonly string ConnectionString;
        private readonly DbProviderFactory ProviderFactory;

        public OrderRepository(string connectionString, string nameProvider)
        {
            ConnectionString = connectionString;
            ProviderFactory = DbProviderFactories.GetFactory(nameProvider);
        }

        public void Delete(int guid)
        {
            throw new NotImplementedException();
        }

        public Order GetBy(int guid)
        {
            Order result = default;

            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "select  OrderID," +
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

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@id";
                    parameter.DbType = DbType.Int32;
                    parameter.Value = guid;

                    command.Parameters.Add(parameter);
                    command.CommandType = CommandType.Text;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = FromReaderToOrder(reader);
                        }
                    }
                }
            }

            return result;
        }

        public IList<Order> GetCollection(string[] includesFields = null)
        {
            IList<Order> result = new List<Order>();

            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "select  OrderID," +
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

                    command.CommandType = CommandType.Text;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(FromReaderToOrder(reader));
                        }
                    }
                }
            }

            return result;
        }

        public Order Insert(Order item)
        {
            throw new NotImplementedException();
        }

        public Order Update(Order item)
        {
            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "" +
                        "UPDATE table_name" +
                        "SET " +
                        "   column1 = value1, " +
                        "   column2 = value2, " +
                        "WHERE OrderId=@id";

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@id"; 
                    parameter.DbType = DbType.Int32; 
                    command.Parameters.Add(parameter);
                }
            }

        }

        bool IRepository<Order>.Delete(int id)
        {
            bool isDeleted = false;

            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM dbo.Orders WHERE OrderId=@id";
                    var parameter = command.CreateParameter();
                    parameter.Value = id;
                    parameter.DbType = DbType.Int32;
                    command.Parameters.Add(parameter);
                    isDeleted = command.ExecuteNonQuery() > 0;
                }
            }
        
            return isDeleted;
        }

        private Order FromReaderToOrder(DbDataReader reader)
        {
            var order = new Order(
                orderDate: reader.SafeCastNullableDateTime(3),
                shippedDate: reader.SafeCastNullableDateTime(5),
                status: (OrderStatus) reader.GetInt32(14)
            );

            order.OrderId = reader.SafeCastInt32(0);
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