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

        public OrderService(IOrderRepository orderRepository)
        {
            OrderRepository = orderRepository;
        }

        public void Create(Order obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            return OrderRepository.Delete(id);
        }

        public int DeleteNotCompletedOrders()
        {
            return OrderRepository.DeleteNotCompletedOrders();
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
            throw new NotImplementedException();
        }

        public void MoveToProgress(int id)
        {
            throw new NotImplementedException();
        }

        public Order Update(Order obj)
        {
            throw new NotImplementedException();
        }
    }
}
