using OrderManagement.DataAccess.Contract.Models;

namespace OrderManagement.DataAccess.Contract.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        int DeleteNotCompletedOrders();
    }
}
