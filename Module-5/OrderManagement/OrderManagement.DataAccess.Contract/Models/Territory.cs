using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.DataAccess.Contract.Models
{
    [Table("Territories")]
    public class Territory
    {
        public int TerritoryID { get; set; }

        public string TerritoryDescription { get; set; }

        public int RegionId { get; set; }

        public IList<Region> Regions { get; set; }
    }
}
