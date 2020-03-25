using OrderManagement.DataAccess.Contract.Interfaces;
using OrderManagement.DataAccess.Contract.Models;
using OrderManagement.DataAccess.Contract.Models.Statistic;
using OrderManagement.DataAccess.Extensions;
using OrderManagement.DataAccess.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace OrderManagement.DataAccess
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        protected readonly string ConnectionString;
        protected readonly DbProviderFactory ProviderFactory;

        private readonly string InsertSql = OrderDetailSqls.Insert;
        private readonly string UpdateSql = OrderDetailSqls.Update;

        public OrderDetailRepository(string connectionString, string nameProvider)
        {
            ConnectionString = connectionString;
            ProviderFactory = DbProviderFactories.GetFactory(nameProvider);
        }

        public int Delete(int orderId)
        {
            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "delete from dbo.[Order Details] where OrderId=@orderId";
                    command.AddParameter("@orderId", DbType.Int32, orderId);
                    return command.ExecuteNonQuery();
                }
            }
        }

        public bool Delete(int orderId, int productId)
        {
            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "delete from dbo.[Order Details] where OrderId=@orderId and ProductId=@prodId";
                    command.AddParameter("@orderId", DbType.Int32, orderId);
                    command.AddParameter("@prodId", DbType.Int32, productId);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public OrderDetail Get(int orderId, int productId)
        {
            OrderDetail orderDetail = null;

            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open(); 
                
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from dbo.[Order Details] where OrderId=@orderId and ProductId=@prodId";
                    command.AddParameter("@orderId", DbType.Int32, orderId);
                    command.AddParameter("@prodId", DbType.Int32, productId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            orderDetail = new OrderDetail()
                            {
                                OrderId = reader.SafeCastInt32(0),
                                ProductId = reader.SafeCastInt32(1),
                                UnitPrice = reader.SafeCastDecimal(2),
                                Quantity = reader.SafeCastInt16(3),
                                Discount = reader.SafeCastFloat(4)
                            };
                        }
                    }
                }
            }

            return orderDetail;
        }

        public CustOrdersDetail GetCustOrderDetail(int orderId)
        {
            throw new NotImplementedException();
        }

        public CustOrderHist GetCustOrderHist(int customerId)
        {
            throw new NotImplementedException();
        }

        public int InsertDetailsInOrder(int orderId, ICollection<OrderDetail> details)
        {
            if (details == null || details.Count == 0)
            {
                throw new Exception("Details should not be null or empty");
            }

            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.AddParameter("@orderId", DbType.Int32, orderId);

                    var builder = new StringBuilder();
                    for (int i = 0; i < details.Count; ++i)
                    {
                        var detail = details.ElementAt(i);
                        builder.Append(InsertSql);
                        command.AddParameter($"@productId_{i}", DbType.Int32, detail.ProductId);
                        command.AddParameter($"@unitPrice_{i}", DbType.Decimal, detail.UnitPrice);
                        command.AddParameter($"@qty_{i}", DbType.Int32, detail.Quantity);
                        command.AddParameter($"@discount_{i}", DbType.Single, detail.Discount);
                    }

                    command.CommandText = builder.ToString();
                    return command.ExecuteNonQuery();
                }
            }
        }

        public void Update(OrderDetail detail)
        {
            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = UpdateSql;
                    command.AddParameter("@orderId", DbType.Int32, detail.OrderId);
                    command.AddParameter("@productId", DbType.Int32, detail.ProductId);
                    command.AddParameter("@unitPrice", DbType.Decimal, detail.UnitPrice);
                    command.AddParameter("@qty", DbType.Int16, detail.Quantity);
                    command.AddParameter("@discount", DbType.Single, detail.Discount);

                    if (command.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("yyyps sorry, something went wrong");
                    }
                }
            }
        }
    }
}
