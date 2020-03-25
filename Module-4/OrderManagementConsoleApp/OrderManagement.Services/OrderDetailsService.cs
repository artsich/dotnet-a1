using OrderManagement.DataAccess.Contract.Interfaces;
using OrderManagement.DataAccess.Contract.Models;
using OrderManagement.DataAccess.Exceptions;
using OrderManagement.Services.Interfaces;
using System.Collections.Generic;

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
            var detail = OrderDetailRepository.Get(obj.OrderId, obj.ProductId);
            if (detail != null)
            {
                throw new InsertEntityException("Entity already exist.");
            }

            OrderDetailRepository.InsertDetailsInOrder(obj.OrderId, new List<OrderDetail> { obj });
        }

        public bool Delete(int orderId, int productId)
        {
            var detail = OrderDetailRepository.Get(orderId, productId);

            if (detail == null)
            {
                throw new NotFoundEntityException($"Order detail with orderId: {orderId}, productId: {productId}");
            }

            return OrderDetailRepository.Delete(orderId, productId);
        }

        public OrderDetail GetById(int orderId, int productId)
        {
            var detail = OrderDetailRepository.Get(orderId, productId);

            if (detail == null)
            {
                throw new NotFoundEntityException($"Order detail with orderId: {orderId}, productId: {productId}");
            }

            return detail;
        }

        public void Update(OrderDetail obj)
        {
            GetById(obj.OrderId, obj.ProductId);

            OrderDetailRepository.Update(obj);
        }
    }
}
