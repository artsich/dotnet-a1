using OrderManagement.DataAccess.Contract.Models;
using OrderManagement.DataAccess.Contract.Models.Statistic;
using System.Collections.Generic;

namespace OrderManagement.DataAccess.Contract.Interfaces
{
    public interface IOrderDetailRepository
    {
        void Update(OrderDetail detail);

        void Delete(int orderId, int productId);

        void Get(int orderId, int productId);

        int InsertDetailsInOrder(int orderId, ICollection<OrderDetail> details);

        CustOrderHist GetCustOrderHist(int customerId);

        CustOrdersDetail GetCustOrderDetail(int orderId);
    }
}
