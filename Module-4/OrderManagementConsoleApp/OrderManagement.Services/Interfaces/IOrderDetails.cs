using OrderManagement.DataAccess.Contract.Models;
using System.Collections.Generic;

namespace OrderManagement.Services.Interfaces
{
    public interface IOrderDetails
    {
        OrderDetail GetById(int orderId, int productId);

        IList<OrderDetail> GetCollection();

        void Create(OrderDetail obj);

        void Update(OrderDetail obj);

        bool Delete(int orderId, int productId);
    }
}
