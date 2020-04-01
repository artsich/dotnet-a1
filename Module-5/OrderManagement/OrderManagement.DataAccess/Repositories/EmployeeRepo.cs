using Dapper;
using OrderManagement.DataAccess.Contract.Interfaces;
using OrderManagement.DataAccess.Contract.Models;
using OrderManagement.DataAccess.Contract.Models.Db;
using OrderManagement.DataAccess.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace OrderManagement.DataAccess.Repositories
{
    public class EmployeeRepo : AbstractRepository<Employee>, IEmployeeRepository
    {
        private const string Sql_GetStatByRegions = @"
            select 
	            R.RegionID,
	            R.RegionDescription,
	            count (distinct E.EmployeeID) as 'Count'
            from [dbo].[Regions] as R
            inner join [dbo].Territories as T on T.RegionID = R.RegionID
            inner join [dbo].EmployeeTerritories as E on E.TerritoryID = T.TerritoryID
            group by R.RegionID, R.RegionDescription;";

        private const string Sql_GetEmplsWithShips = @"
            select distinct E.EmployeeID
	            , E.[Address]
	            , E.BirthDate
	            , E.City
	            , E.Country
	            , E.Extension
	            , E.FirstName
	            , E.HireDate
	            , E.HomePhone
	            , E.LastName
	            , convert(nvarchar(max),E.Notes) as 'Notes'
	            , cast(E.Photo as varbinary(max)) as 'Photo'
	            , E.PhotoPath
	            , E.PostalCode
	            , E.Region
	            , E.ReportsTo
	            , E.Title
	            , E.TitleOfCourtesy

	            , S.ShipperID
	            , S.CompanyName
	            , S.Phone
            from [dbo].[Employees] as E
            inner join [dbo].[Orders] as O on O.EmployeeID = E.EmployeeID
            inner join [dbo].[Shippers] as S on S.ShipperID = O.ShipVia
            order by E.EmployeeID;";

        private const string Sql_GetEmplManagedTerritories = @"
            select distinct E.EmployeeID
	            , E.[Address]
	            , E.BirthDate
	            , E.City
	            , E.Country
	            , E.Extension
	            , E.FirstName
	            , E.HireDate
	            , E.HomePhone
	            , E.LastName
	            , convert(nvarchar(max),E.Notes) 
	            , cast(E.Photo as varbinary(max)) as [E.Photo]
	            , E.PhotoPath
	            , E.PostalCode
	            , E.Region
	            , E.ReportsTo
	            , E.Title
	            , E.TitleOfCourtesy
	            , T.TerritoryID
	            , T.TerritoryDescription
            from [dbo].[Regions] as R
            inner join [dbo].Territories as T on T.RegionID = R.RegionID
            inner join [dbo].EmployeeTerritories as ET on ET.TerritoryID = T.TerritoryID
            inner join [dbo].Employees as E on E.EmployeeID = ET.EmployeeID;";

        public EmployeeRepo(string connString, string providerName)
            : base(connString, providerName)
        {
        }

        public IList<StatByRegion> GetStatByRegions()
        {
            using (var connection = ProviderFactory.CreateConnection(ConnectionString))
            {
                var result = connection.Query<Region, int, StatByRegion>(
                    Sql_GetStatByRegions,
                    (r, c) => new StatByRegion() { Region = r, Count = c },
                    splitOn: "RegionID,Count")
                .ToList();
                return result;
            }
        }

        public IList<Employee> GetEmplManagedTerritories()
        {
            using (var connection = ProviderFactory.CreateConnection(ConnectionString))
            {
                var list = connection.Query<Employee, Territory, (Employee ee, Territory tt)>(
                    Sql_GetEmplManagedTerritories,
                    (e, t) => (e, t),
                    splitOn: "EmployeeID,TerritoryID");

                var result = list?.GroupBy(e => e.ee.EmployeeId)
                    .Select(x =>
                    {
                        var empl = x.First().ee;
                        empl.Territories = x.Select(r => r.tt).ToList();
                        return empl;
                    }).ToList()
                ;

                return result;
            }
        }

        public IList<EmplShipWorked> GetEmplsWithShips()
        {
            using (var connection = ProviderFactory.CreateConnection(ConnectionString))
            {
                var emplDict = new Dictionary<int, EmplShipWorked>();

                connection.Query<Employee, Shipper, Shipper>(
                    Sql_GetEmplsWithShips,
                    (e, s) =>
                    {
                        if (!emplDict.TryGetValue(e.EmployeeId, out var empl))
                        {
                            emplDict[e.EmployeeId] = new EmplShipWorked()
                            {
                                Employee = e,
                                Ships = new List<Shipper>()
                            };
                        }

                        emplDict[e.EmployeeId].Ships.Add(s);
                        return s;
                    },
                    splitOn: "EmployeeID,ShipperID");

                return emplDict.Values.ToList();
            }
        }
    }
}
