using OrderManagement.DataAccess.Models.Db;

namespace OrderManagement.DataAccess.Models
{
    public class StatByRegion
    {
        public int Count { get; set; }

        public Region Region { get; set; }
    }
}
