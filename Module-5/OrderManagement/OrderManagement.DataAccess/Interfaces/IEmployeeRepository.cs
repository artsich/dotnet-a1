using OrderManagement.DataAccess.Contract.Models;
using OrderManagement.DataAccess.Models;
using OrderManagement.DataAccess.Models.Db;
using System.Collections.Generic;

namespace OrderManagement.DataAccess.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        IList<StatByRegion> GetStatByRegions();

        IList<EmplShipWorked> GetEmplsWithShips();
    }
}