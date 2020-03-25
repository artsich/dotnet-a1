using OrderManagement.DataAccess.Contract.Interfaces;
using OrderManagement.DataAccess.Contract.Models;
using OrderManagement.DataAccess.Contract.Models.Statistic;
using OrderManagement.DataAccess.Exceptions;
using OrderManagement.DataAccess.Extensions;
using OrderManagement.DataAccess.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace OrderManagement.DataAccess
{
    public class OrderRepository : AdoAbstractRepository<Order>, IOrderRepository
    {
        private readonly string UpdateSql = OrderSqls.UpdateSql;
        private readonly string InsertSql = OrderSqls.InsertSql;
        private readonly string MarkAsDoneSql = OrderSqls.MarkAsDone;
        private readonly string MoveToProgressSql = OrderSqls.MoveToProgress;

        protected override string GetByIdSql => OrderSqls.GetByIdSql;

        protected override string DeleteSql => OrderSqls.DeleteSql;

        protected override string GetCollectionSql => OrderSqls.GetCollectionSql;

        public OrderRepository(string connectionString, string nameProvider) :
            base(connectionString, nameProvider)
        {
        }

        public void MarkAsDone(int id, DateTime dateTime)
        {
            SetupDateInAttribute(id, dateTime, MarkAsDoneSql);
        }

        public void MoveToProgress(int id, DateTime dateTime)
        {
            SetupDateInAttribute(id, dateTime, MoveToProgressSql);
        }

        public CustOrderHist GetCustOrderHist(int customerId)
        {
            return new CustOrderHist();
        }

        public CustOrdersDetail GetCustOrderDetail(int orderId)
        {
            return new CustOrdersDetail();
        }

        public override Order GetBy(int guid)
        {
            Order result = default;
            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = GetByIdSql;
                    command.AddParameter(ParamIdName, DbType.Int32, guid);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = FromReaderToObject(reader);

                            if (reader.NextResult())
                            {
                                result.OrderDetails = ParseOrderDetail(reader);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public override Order Insert(Order item)
        {
            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = InsertSql;
                    PrepareInsertUpdateCommand(item, command);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var orderId = reader.SafeCastInt32(0);
                            item.Id = (int)orderId;
                        }
                        else
                        {
                            throw new Exception("Id of last record has not returned");
                        }
                    }

                }
            }

            return item;
        }

        public override void Update(Order item)
        {
            if (item.Status == OrderStatus.IsDone || item.Status == OrderStatus.InProgress)
            {
                throw new UpdateEntityException("You can't update `Completed` or `in work` order");
            }

            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = UpdateSql;
                    PrepareInsertUpdateCommand(item, command);

                    if (command.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("yyyyyps");
                    }
                }
            }
        }

        private void PrepareInsertUpdateCommand(Order item, DbCommand command)
        {
            command.AddParameter("@custId", DbType.String, item.CustomerId);
            command.AddParameter("@emplId", DbType.Int32, item.EmployeeId);
            command.AddParameter("@reqDate", DbType.DateTime, item.RequiredDate);
            command.AddParameter("@shipVia", DbType.Int32, item.ShipVia);
            command.AddParameter("@freight", DbType.Decimal, item.Freight);
            command.AddParameter("@shipName", DbType.String, item.ShipName);
            command.AddParameter("@shipAddress", DbType.String, item.ShipAddress);
            command.AddParameter("@shipCity", DbType.String, item.ShipCity);
            command.AddParameter("@shipRegion", DbType.String, item.ShipRegion);
            command.AddParameter("@shipPostalCode", DbType.String, item.ShipPostalCode);
            command.AddParameter("@shipCntry", DbType.String, item.ShipCountry);
        }

        private void SetupDateInAttribute(int id, DateTime dateTime, string sql)
        {
            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.AddParameter(ParamIdName, DbType.Int32, id);
                    command.AddParameter("@dateTime", DbType.DateTime, dateTime);
                    command.ExecuteNonQuery();
                }
            }
        }

        private IList<OrderDetail> ParseOrderDetail(DbDataReader reader)
        {
            var result = new List<OrderDetail>();

            while (reader.Read())
            {
                result.Add(new OrderDetail()
                {
                    OrderId = reader.SafeCastInt32(0),
                    ProductId = reader.SafeCastInt32(1),
                    UnitPrice = reader.SafeCastDecimal(2),
                    Quantity = reader.SafeCastInt16(3),
                    Discount = reader.SafeCastFloat(4)
                });
            }

            return result;
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