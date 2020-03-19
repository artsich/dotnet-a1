using System.Collections.Generic;
using OrderManagement.DataAccess.Contract.Models;

namespace OrderManagement.Services.Interfaces
{
    public interface IOrderService
    {
        Order CreateOrder();

        Order GetOrder(int id);

        IList<Order> GetOrders();

        IList<Order> GetLiteOrdersInfo();

        void MoveOrderStatusToWork(Order order);

        void MoveOrderStatusToCompleted(Order order);

        void DeleteNotCompletedOrders();
    }
}
