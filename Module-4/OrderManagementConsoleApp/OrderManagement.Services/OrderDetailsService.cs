using OrderManagement.DataAccess.Contract.Interfaces;
using OrderManagement.DataAccess.Contract.Models;
using OrderManagement.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Services
{
    public class OrderDetailsService : IOrderDetails
    {
        private readonly IOrderDetailRepository OrderDetailRepository;

        public OrderDetailsService(IOrderDetailRepository orderDetailRepository)
        {
            OrderDetailRepository = orderDetailRepository;
        }

        public void Create(OrderDetail obj)
        {
            GetById(obj.OrderId, obj.ProductId);

            //sorry, just laziness write single method insert
            OrderDetailRepository.InsertDetailsInOrder(obj.OrderId, new List<OrderDetail> { obj });
        }

        public bool Delete(int orderId, int productId)
        {
            throw new NotImplementedException();
        }

        public OrderDetail GetById(int orderId, int productId)
        {
            throw new NotImplementedException();
        }

        public IList<OrderDetail> GetCollection()
        {
            throw new NotImplementedException();
        }

        public void Update(OrderDetail obj)
        {
            GetById(obj.OrderId, obj.ProductId);

            OrderDetailRepository.Update(obj);
        }
    }
}
