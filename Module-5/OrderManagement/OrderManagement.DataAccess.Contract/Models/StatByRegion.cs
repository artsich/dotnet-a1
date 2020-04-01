using OrderManagement.DataAccess.Contract.Models.Db;

namespace OrderManagement.DataAccess.Contract.Models
{
    public class StatByRegion
    {
        public int Count { get; set; }

        public Region Region { get; set; }
    }
}
