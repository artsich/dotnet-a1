using OrderManagement.DataAccess.Interfaces;
using OrderManagement.DataAccess.Models.Db;

namespace OrderManagement.DataAccess.Repositories
{
    public class TerritoryRepo : AbstractRepository<Territory>, ITerritoryRepo
    {
        protected override string Sql_TryInsertMany => @"
            if not exists (select * from dbo.Territories where TerritoryID=@TerritoryID)
	            insert into dbo.Territories (TerritoryID, TerritoryDescription, RegionID)
	            values (@TerritoryID, @TerritoryDescription, @RegionID);";

        public TerritoryRepo(string connString, string providerName)
            : base(connString, providerName)
        {
        }
    }
}
