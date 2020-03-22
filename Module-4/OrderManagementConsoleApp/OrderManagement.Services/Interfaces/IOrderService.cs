using System.Collections.Generic;
using OrderManagement.DataAccess.Contract.Models;

namespace OrderManagement.Services.Interfaces
{
    public interface IOrderService : IService<Order>
    {
        int DeleteNotCompletedOrders();

        void MoveToProgress(int id);

        void MarkDone(int id);
    }
}
