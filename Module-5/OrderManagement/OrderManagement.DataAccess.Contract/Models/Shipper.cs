using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.DataAccess.Contract.Models
{
    [Table("Shippers")]
    public class Shipper
    {
        public int ShipperID { get; set; }

        public string CompanyName { get; set; }

        public string Phone { get; set; }
    }
}
