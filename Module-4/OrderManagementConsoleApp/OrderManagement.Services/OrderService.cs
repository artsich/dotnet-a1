using OrderManagement.DataAccess.Contract;
using OrderManagement.DataAccess.Contract.Models;
using OrderManagement.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace OrderManagement.Services
{

    public static class QueryExtension
    {
        public static IQuery<T> If<T>(this IQuery<T> query, Expression<Predicate<Order>> expression)
        {

            return query;
        }
    }

    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> OrderRepository;

        public OrderService(IRepository<Order> orderRepository)
        {
            OrderRepository = orderRepository;
        }

        public Order CreateOrder()
        {
            throw new NotImplementedException();
        }

        public Order GetOrder(int id)
        {
            return OrderRepository.GetBy(id);
        }

        public void DeleteNotCompletedOrders()
        {
                OrderRepository<T>()
                .Delete()
                .If(x => x.Status=OrderStatus.New || x.Status=OrderStatus.InWork)
                .Execute();

            throw new NotImplementedException();
        }

        public IList<Order> GetLiteOrdersInfo()
        {
            throw new NotImplementedException();
        }

        public IList<Order> GetOrders()
        {
            return OrderRepository.GetCollection().ToList();
        }

        public void MoveOrderStatusToCompleted(Order order)
        {
            throw new NotImplementedException();
        }

        public void MoveOrderStatusToWork(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
