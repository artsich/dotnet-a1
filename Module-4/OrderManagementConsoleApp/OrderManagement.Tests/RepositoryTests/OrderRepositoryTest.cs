using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderManagement.DataAccess;
using OrderManagement.DataAccess.Contract.Models;
using OrderManagement.DataAccess.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Tests.RepositoryTests
{
    [TestClass]
    public class OrderRepositoryTest
    {
        OrderRepository repos = new OrderRepository(Settings.CollectionString, Settings.ProviderName);

        [TestMethod]
        public void GivenOrderWithNullFields_InsertOrder_ReturnOrderWithUpdatedId()
        {
            var order = new Order();
            var oldId = order.Id;
            repos.Insert(order);
            order.Id.Should().NotBe(oldId);
        }

        [TestMethod]
        public void GivenOrderId_GetOrderBy_ReturnOrderWithAccordingId()
        {
            var order = repos.GetBy(10248);
            order.Id.Should().Be(10248);
        }

        [TestMethod]
        public void GivenOrderOrderFromDb_TryToUpdate_ReturnUpdatedOrder()
        {
            var order = repos.GetBy(11091);
            order.ShipCountry = "Grodno";
            repos.Update(order);
            var updatedFromDb = repos.GetBy(order.Id);
            order.ShipCountry.Should().Be(updatedFromDb.ShipCountry);
        }

        [TestMethod]
        public void GivenOrderOrderFromDbWhichIsDone_TryToUpdate_ReturnException()
        {
            var order = repos.GetBy(10248);
            order.ShipCountry = "dasdsad";
            Assert.ThrowsException<UpdateEntityException>(() => repos.Update(order));
        }

        [TestMethod]
        public void GivenOrderWithStatusNew_MoveToInProgress_ReturnOrderWithUpdatedOrderId()
        {
            repos.MarkAsDone(11100, DateTime.Now);
            var order = repos.GetBy(11100);
            order.Status.Should().Be(OrderStatus.InProgress);
        }

        [TestMethod]
        public void GivenOrderWithStatusInProgress_MarkAsDone_ReturnOrderWithUpdatedShippedDate()
        {
            repos.MarkAsDone(11077, DateTime.Now);
            var order = repos.GetBy(11077);
            order.Status.Should().Be(OrderStatus.IsDone);
        }
    }
}
