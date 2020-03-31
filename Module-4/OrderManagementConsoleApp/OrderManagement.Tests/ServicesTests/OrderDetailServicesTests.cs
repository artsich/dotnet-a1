using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrderManagement.DataAccess.Contract.Interfaces;
using OrderManagement.DataAccess.Contract.Models;
using OrderManagement.DataAccess.Exceptions;
using OrderManagement.Services;

namespace OrderManagement.Tests.ServicesTests
{
    [TestClass]
    public class OrderDetailServicesTests
    {
        [TestMethod]
        public void GivenAlreadyHaveCreatedOrderDetailEntity_TryToCreateAgain_ReturnInsertEntityException()
        {
            var mockOrderDetRep = new Mock<IOrderDetailRepository>();
            mockOrderDetRep.Setup(x => x.Get(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => new OrderDetail() { OrderId = 1, ProductId = 1 });

            var service = new OrderDetailsService(mockOrderDetRep.Object);
            Assert.ThrowsException<InsertEntityException>(() => service.Create(new OrderDetail() { OrderId = 1, ProductId = 1 }));
        }
    }
}
