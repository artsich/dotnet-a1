using OrderManagement.DataAccess.Contract.Models;
using System.Collections.Generic;

namespace OrderManagement.DataAccess.Contract.Interfaces
{
    public interface IOrderDetailRepository
    {
        int InsertDetailsInOrder(int orderId, ICollection<OrderDetail> details);
    }
}
