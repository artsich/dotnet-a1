using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.DataAccess.Contract.Models.Db
{
    [Table("Territories")]
    public class Territory
    {
        public int TerritoryID { get; set; }

        public string TerritoryDescription { get; set; }

        public int RegionId { get; set; }

        public Region Region { get; set; }
    }
}
