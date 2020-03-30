using Dapper;
using OrderManagement.DataAccess.Contract.Interfaces;
using OrderManagement.DataAccess.Contract.Models;
using OrderManagement.DataAccess.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace OrderManagement.DataAccess
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly string ConnectionString;
        private readonly string ProviderName;
        private readonly DbProviderFactory ProviderFactory;

        public OrderRepository(string connectionString, string providerName)
        {
            ConnectionString = connectionString;
            ProviderName = providerName;
            ProviderFactory = DbProviderFactories.GetFactory(providerName);
        }

        public Order Get(int id)
        {
            using (var connection = ProviderFactory.CreateConnection(ConnectionString))
            {
                var reader = connection.QueryMultiple(
                    "select * from dbo.Orders where OrderId=@orderId;" +
                    "select * from dbo.[Order Details] where OrderId=@orderId;",
                    new
                    {
                        @orderId = id
                    }
                );

                var order = reader.Read<Order>().FirstOrDefault();
                var orderDetail = reader.Read<OrderDetail>().ToList();
                orderDetail.ForEach(x =>
                {
                    x.Order = order;

                });
            }
        }

        public void Delete(Order item)
        {
            throw new NotImplementedException();
        }

        public IList<Order> GetAll()
        {
            throw new NotImplementedException();
        }

        public Order Insert(Order item)
        {
            throw new NotImplementedException();
        }

        public void Update(Order item)
        {
            throw new NotImplementedException();
        }
    }
}