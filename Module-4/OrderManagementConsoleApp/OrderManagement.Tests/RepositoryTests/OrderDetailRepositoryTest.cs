using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderManagement.DataAccess;
using OrderManagement.DataAccess.Contract.Models;
using System.Collections.Generic;

namespace OrderManagement.Tests.RepositoryTests
{
    [TestClass]
    public class OrderDetailRepositoryTest
    {
        // public void Update(OrderDetail detail)

        OrderDetailRepository repos = new OrderDetailRepository(Settings.CollectionString, Settings.ProviderName);

        [TestMethod]
        public void GivenOrderId_DeleteAllOrderDetails_ReturnCountOfDeletedRows()
        {
            repos.Delete(10624).Should().Be(3);
        }

        [TestMethod]
        public void GivenOrderIdProductId_DeleteOneRow_ReturnTrueResult()
        {
            repos.Delete(10625, 14).Should().Be(true);
        }

        [TestMethod]
        public void GivenListOrderDetals_InsertDetailForOrder_ReturnCountAddedRows()
        {
            var o1 = new OrderDetail() { OrderId = 10624, ProductId = 28, UnitPrice = 45, Quantity = 10, Discount = 0 };
            var o2 = new OrderDetail() { OrderId = 10624, ProductId = 29, UnitPrice = 123, Quantity = 6, Discount = 0 };
            var o3 = new OrderDetail() { OrderId = 10624, ProductId = 44, UnitPrice = 19, Quantity = 10, Discount = 0 };

            repos.InsertDetailsInOrder(10624, new List<OrderDetail>() { o1, o2, o3 }).Should().Be(3);
        }

        [TestMethod]
        public void GivenOrderDetalsFromDb_UpdateQuantity_ReturnUpdateOrderDetail()
        {
            var o1 = repos.Get(10623, 35);
            o1.Quantity = 5522;
            repos.Update(o1);
            var o2 = repos.Get(10623, 35);
            o2.Quantity.Should().Be(5522);
        }
    }
}