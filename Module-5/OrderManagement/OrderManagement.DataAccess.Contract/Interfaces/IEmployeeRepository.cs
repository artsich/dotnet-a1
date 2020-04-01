using OrderManagement.DataAccess.Contract.Models;
using OrderManagement.DataAccess.Contract.Models.Db;
using System.Collections.Generic;

namespace OrderManagement.DataAccess.Contract.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        IList<StatByRegion> GetStatByRegions();

        IList<EmplShipWorked> GetEmplsWithShips();

        IList<Employee> GetEmplManagedTerritories();
    }
}
