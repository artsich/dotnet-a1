using OrderManagement.DataAccess.Contract.Models;
using System;

namespace OrderManagement.DataAccess.Contract.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        void MarkAsDone(int id, DateTime dateTime);

        void MoveToProgress(int id, DateTime dataTime);
    }
}
