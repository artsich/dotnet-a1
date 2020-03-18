using OrderManagement.DataAccess.Contract;
using OrderManagement.DataAccess.Contract.Models;
using System;
using System.Collections.Generic;
using System.Text;
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
            throw new NotImplementedException();
        }

        public IList<Order> GetCollection()
        {
            IList<Order> result = new List<Order>();

            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select OrderID, OrderDate from dbo.Orders";
                    command.CommandType = CommandType.Text;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Order()
                            {
                                OrderId = reader.GetInt32(0),
                                OrderDate = reader.GetDateTime(1)
                            });
                        }
                    }
                }
            }

            return result;
        }

        public void Insert(Order item)
        {
            throw new NotImplementedException();
        }

        public void Update(Order item)
        {
            throw new NotImplementedException();
        }
    }
}