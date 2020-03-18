using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.DataAccess.Contract.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
