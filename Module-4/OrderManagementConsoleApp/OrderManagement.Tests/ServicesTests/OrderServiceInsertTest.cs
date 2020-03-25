using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrderManagement.DataAccess.Contract.Interfaces;
using OrderManagement.DataAccess.Contract.Models;
using OrderManagement.DataAccess.Exceptions;
using OrderManagement.Services;

namespace OrderManagement.Tests
{
    [TestClass]
    public class OrderServiceTest
    {
        [TestMethod]
        public void GivenNotExistOrderEntity_CallInsertMethod_ReturnInsertEntityException()
        {
            var mockOrderRep = new Mock<IOrderRepository>();
            mockOrderRep
                .Setup(x => x.Insert(It.IsAny<Order>()))
                .Returns(() => null);

            var mockOrderDetRep = new Mock<IOrderDetailRepository>();

            var service = new OrderService(mockOrderRep.Object, mockOrderDetRep.Object);

            Assert.ThrowsException<InsertEntityException>(() => service.Create(new Order()));
        }

        [TestMethod]
        public void GevenOrderWithInvalidId_TryToGetById_ReturnNotFoundException()
        {
            var mockOrderRep = new Mock<IOrderRepository>();
            mockOrderRep
                .Setup(x => x.GetBy(It.IsAny<int>()))
                .Returns(() => null);

            var mockOrderDetRep = new Mock<IOrderDetailRepository>();
            var service = new OrderService(mockOrderRep.Object, mockOrderDetRep.Object);

            Assert.ThrowsException<NotFoundEntityException>(() => service.GetById(8));
        }

        [TestMethod]
        public void GivenOrderWithStatusDone_TryToMarkAsDone_ReturnUpdateEntityException()
        {
            var mockOrderRep = new Mock<IOrderRepository>();
            mockOrderRep
                .Setup(x => x.GetBy(It.IsAny<int>()))
                .Returns(() => new Order() { Status = OrderStatus.IsDone });

            var mockOrderDetRep = new Mock<IOrderDetailRepository>();
            var service = new OrderService(mockOrderRep.Object, mockOrderDetRep.Object);

            Assert.ThrowsException<UpdateEntityException>(() => service.MarkDone(0));
        }

        [TestMethod]
        public void GivenOrderWithStatusInProggress_TryMoveInProgress_ReturnUpdateEntityException()
        {
            var mockOrderRep = new Mock<IOrderRepository>();
            mockOrderRep
                .Setup(x => x.GetBy(It.IsAny<int>()))
                .Returns(() => new Order() { Status = OrderStatus.InProgress });

            var mockOrderDetRep = new Mock<IOrderDetailRepository>();
            var service = new OrderService(mockOrderRep.Object, mockOrderDetRep.Object);

            Assert.ThrowsException<UpdateEntityException>(() => service.MoveToProgress(0));
        }
    }
}
