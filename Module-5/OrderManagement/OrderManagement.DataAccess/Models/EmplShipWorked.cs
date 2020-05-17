using OrderManagement.DataAccess.Models.Db;
using System.Collections.Generic;

namespace OrderManagement.DataAccess.Contract.Models
{
    public class EmplShipWorked
    {
        public Employee Employee { get; set; }

        public IList<Shipper> Ships { get; set; }
    }
}
