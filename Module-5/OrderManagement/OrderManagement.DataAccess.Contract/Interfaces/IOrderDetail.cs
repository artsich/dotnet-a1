using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.DataAccess.Contract.Interfaces
{
    public interface IOrderDetailRepo
    {
        bool ReplaceProduct(int orderId, int fromProductId, int toProductId);
    }
}
