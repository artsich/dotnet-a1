using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.DataAccess.Contract.Models.Db
{
    [Table("CustomerCustomerDemo")]
    public class CustomerDemo
    {
        [ForeignKey("CustomerID")]
        public string CustomerID { get; set; }

        [ForeignKey("CustomerTypeID")]
        public string CustomerTypeID { get; set; }

        public CustomerDemographic CustomerDemographic { get; set; }
    }
}
