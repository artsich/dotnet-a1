using System.Collections.Generic;

namespace OrderManagement.DataAccess.Models.Db
{
    public class Region
    {
        public int RegionID { get; set; }

        public string RegionDescription { get; set; }

        public IList<Territory> Territories { get; set; }
    }
}