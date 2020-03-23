using OrderManagement.DataAccess.Contract.Interfaces;
using OrderManagement.DataAccess.Contract.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace OrderManagement.DataAccess
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        protected readonly string ConnectionString;
        protected readonly DbProviderFactory ProviderFactory;

        public OrderDetailRepository(string connectionString, string nameProvider)
        {
            ConnectionString = connectionString;
            ProviderFactory = DbProviderFactories.GetFactory(nameProvider);
        }

        public int InsertDetailsInOrder(int orderId, ICollection<OrderDetail> details)
        {
            if (details == null || details.Count == 0)
            {
                throw new System.Exception("Details should not be null or empty");
            }

            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    var orderIdParam = command.CreateParameter();
                    orderIdParam.ParameterName = "@orderId";
                    orderIdParam.DbType = DbType.Int32;
                    orderIdParam.Value = orderId;
                    command.Parameters.Add(orderIdParam);

                    var builder = new System.Text.StringBuilder();
                    for (int i = 0; i < details.Count; ++i)
                    {
                        var detail = details.ElementAt(i);
                        builder.Append($"insert into dbo.[Order Details] (OrderID, ProductID, UnitPrice, Quantity, Discount) VALUES(@orderId, @productId_{i}, @unitPrice_{i}, @qty_{i}, @discount_{i});");

                        var productParam = command.CreateParameter();
                        productParam.DbType = DbType.Int32;
                        productParam.ParameterName = $"@productId_{i}";
                        productParam.Value = detail.ProductId;

                        var unitPriceParam = command.CreateParameter();
                        unitPriceParam.DbType = DbType.Decimal;
                        unitPriceParam.ParameterName = $"@unitPrice_{i}";
                        unitPriceParam.Value = detail.UnitPrice;

                        var qtyParam = command.CreateParameter();
                        qtyParam.DbType = DbType.Int32;
                        qtyParam.ParameterName = $"@qty_{i}";
                        qtyParam.Value = detail.Quantity;

                        var discountParam = command.CreateParameter();
                        discountParam.DbType = DbType.Double;
                        discountParam.ParameterName = $"@discount_{i}";
                        discountParam.Value = detail.Discount;

                        command.Parameters.Add(productParam);
                        command.Parameters.Add(unitPriceParam);
                        command.Parameters.Add(qtyParam);
                        command.Parameters.Add(discountParam);
                    }

                    command.CommandText = builder.ToString();
                    return command.ExecuteNonQuery();
                }
            }
        }
    }
}
