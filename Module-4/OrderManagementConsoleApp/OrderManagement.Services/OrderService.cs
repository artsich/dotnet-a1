using OrderManagement.DataAccess.Contract.Interfaces;
using OrderManagement.DataAccess.Contract.Models;
using OrderManagement.DataAccess.Exceptions;
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

        public Order Create(Order obj)
        {
            var order = OrderRepository.Insert(obj);

            if (order == null)
            {
                throw new InsertEntityException();
            }

            var detailsCount = order.OrderDetails.Count;
            if (detailsCount > 0)
            {
                var insertedCount = OrderDetailRepository.InsertDetailsInOrder(order.Id, order.OrderDetails);
                if (detailsCount == insertedCount)
                {
                    throw new InsertEntityException("The details not insert fully..");
                }
            }

            return order;
        }

        public void Delete(int id)
        {
            var order = GetById(id);

            var orderIsDeleted = OrderRepository.Delete(id);
            var detailsIsDeleted = OrderDetailRepository.Delete(id);

            //it can be?
            if (!orderIsDeleted)
                throw new EntityNotDeletedException($"The order with {id} not deleted.");
            if (detailsIsDeleted != order.OrderDetails.Count)
                throw new EntityNotDeletedException($"Detals for order id: {id} not deleted.");
        }

        /// <summary>
        /// This method update info only in Order table, the details updated in OrderDetailService.
        /// </summary>
        /// <param name="obj"></param>
        public void Update(Order obj)
        {
            var oldOrder = GetById(obj.Id);
            if (oldOrder.Status == OrderStatus.New)
            {
                OrderRepository.Update(obj);
            }
        }

        public Order GetById(int id)
        {
            var order = OrderRepository.GetBy(id);
            if (order == null)
            {
                throw new NotFoundEntityException($"The order with id: {id}, not found.");
            }

            return order;
        }

        public IList<Order> GetCollection()
        {
            return OrderRepository.GetCollection();
        }

        public void MarkDone(int id)
        {
            var order = GetById(id);
            if (order.Status == OrderStatus.InProgress)
            {
                OrderRepository.MarkAsDone(id, DateTime.Now);
            }
            else
            {
                throw new UpdateEntityException($"Only `{OrderStatus.InProgress}` order can be move to `{OrderStatus.IsDone}`");
            }
        }

        public void MoveToProgress(int id)
        {
            var order = GetById(id);

            if (order.Status == OrderStatus.New)
            {
                OrderRepository.MoveToProgress(id, DateTime.Now);
            }
            else
            {
                throw new UpdateEntityException($"Only `{OrderStatus.New}` order can be move to `{OrderStatus.InProgress}`");
            }
        }
    }
}
