using OrderManagement.DataAccess.Contract.Interfaces;
using OrderManagement.DataAccess.Contract.Models;
using OrderManagement.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace OrderManagement.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository OrderRepository;
        private readonly IOrderDetailRepository OrderDetailRepository;

        public OrderService(
            IOrderRepository orderRepository, 
            IOrderDetailRepository orderDetailRepository)
        {
            OrderRepository = orderRepository;
            OrderDetailRepository = orderDetailRepository;
        }

        public void Create(Order obj)
        {
            OrderRepository.Insert(obj);
        }

        public bool Delete(int id)
        {
            GetById(id);

            return OrderRepository.Delete(id);
        }

        public void Update(Order obj)
        {
            var oldOrder = GetById(obj.Id);

            if (obj.Status == OrderStatus.New)
            {
                OrderRepository.Update(obj);
            }
        }

        public Order GetById(int id)
        {
            var order = OrderRepository.GetBy(id);
            if (order == null)
            {
                throw new Exception($"The entity with id: {id}, not found.");
            }

            return order;
        }

        public IList<Order> GetCollection(Order obj)
        {
            return OrderRepository.GetCollection();
        }

        public void MarkDone(int id)
        {
            GetById(id);
            OrderRepository.MarkAsDone(id, DateTime.Now);
        }

        public void MoveToProgress(int id)
        {
            GetById(id);

            OrderRepository.MoveToProgress(id, DateTime.Now);
        }
    }
}
