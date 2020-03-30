using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.DataAccess.Contract.Models
{
    [Table("CustomerDemographics")]
    public class CustomerDemographic
    {
        [Key]
        public string CustomerTypeID { get; set; }

        public string CustomerDesc { get; set; }
    }
}
