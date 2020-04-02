using DapperExtensions.Mapper;

namespace OrderManagement.DataAccess.Models.Db
{
    public class Territory
    {
        public string TerritoryID { get; set; }

        public string TerritoryDescription { get; set; }

        public int RegionId { get; set; }

        public Region Region { get; set; }
    }

    internal class TerritoryMapping : ClassMapper<Territory>
    {
        public TerritoryMapping()
        {
            Table("[dbo].[Territories]");
            AutoMap();
            UnMap(x => x.Region);
        }
    }
}
