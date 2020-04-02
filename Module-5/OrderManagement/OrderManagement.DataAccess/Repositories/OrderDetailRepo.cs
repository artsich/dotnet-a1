using Dapper;
using OrderManagement.DataAccess.Extensions;
using OrderManagement.DataAccess.Interfaces;
using System;
using System.Data.Common;

namespace OrderManagement.DataAccess.Repositories
{
    public class OrderDetailRepo : IOrderDetailRepo
    {
        protected readonly string ConnectionString;
        protected readonly string ProviderName;
        protected readonly DbProviderFactory ProviderFactory;

        private const string Sql_ChangeProductInOrder = @"
            update dbo.[Order Details]
            set ProductID=@toId
            from dbo.[Order Details] as OD
            inner join dbo.Orders as O on O.OrderID = OD.OrderID
            where OD.OrderId=@orderId and OD.ProductID=@fromId and O.ShippedDate is null;";

        public OrderDetailRepo(string connString, string providerName)
        {
            ConnectionString = connString;
            ProviderName = providerName;
            ProviderFactory = DbProviderFactories.GetFactory(providerName);
        }

        public bool ReplaceProduct(int orderId, int fromProductId, int toProductId)
        {
            using (var connection = ProviderFactory.CreateConnection(ConnectionString))
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var isChanged = connection.Execute(
                            Sql_ChangeProductInOrder, 
                            new
                            {
                                @orderId = orderId,
                                @toId = toProductId,
                                @fromId = fromProductId
                            }, transaction: transaction) > 0;

                        transaction.Commit();
                        return isChanged;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }
    }
}
