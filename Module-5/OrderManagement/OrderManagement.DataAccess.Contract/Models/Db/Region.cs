using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.DataAccess.Contract.Models.Db
{
    [Table("Regions")]
    public class Region
    {
        [Key]
        public int RegionID { get; set; }

        [Column("RegionDescription")]
        public string RegionDescription { get; set; }

        public IList<Territory> Territories { get; set; }
    }
}