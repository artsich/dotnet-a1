using Dapper;
using OrderManagement.DataAccess.Contract.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{

    internal class TerritoryMap : EntityMap<>

    class Program
    {
        const string ConnectionString = "Server=(localdb)\\ProjectsV13;Database=Northwind;Integrated Security=True";

        static void Main(string[] args)
        {
            string sql = "select top 10 * from dbo.Territories inner join dbo.Regions as Region on Region.RegionID = Territories.RegionID;";
            var factory = DbProviderFactories.GetFactory("System.Data.SqlClient");

            var map = new CustomPropertyTypeMap(
                typeof(Territory), 
                (type, columnName) => 
                )

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                var regionDictionary = new Dictionary<int, Territory>();

                connection.QueryMultiple(sql,
                    new
                    {
                        @regionId= 1,
                    }
                );

                var regions = connection.Query<Territory, Region, Territory>(
                    sql, 
                    (territory, region) =>
                    {
                        if (!regionDictionary.TryGetValue(territory.TerritoryID, out Territory territoryEntry))
                        {
                            territoryEntry = territory;
                            territoryEntry.Regions = new List<Region>();
                            regionDictionary[region.RegionID] = territoryEntry;
                        }

                        territoryEntry.Regions.Add(region);
                        return territoryEntry;
                    },
                    splitOn: "RegionID")
                    .Distinct()
                    .ToList();
            }
        }
    }
}
