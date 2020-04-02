using OrderManagement.DataAccess.Interfaces;
using OrderManagement.DataAccess.Models.Db;
using OrderManagement.Services.Interfaces;
using System;

namespace OrderManagement.Services
{
    public class EmplService : IEmployeeService
    {
        private readonly IEmployeeRepository EmplRepo;
        private readonly ITerritoryRepo TerritoryRepo;

        public EmplService(IEmployeeRepository emplRepo, ITerritoryRepo terrRepo)
        {
            EmplRepo = emplRepo;
            TerritoryRepo = terrRepo;
        }

        public void Insert(Employee empl)
        {
            if (EmplRepo.Get(empl.EmployeeId) != null)
            {
                throw new Exception("Entity already exists.");
            }

            if (EmplRepo.Insert(empl) == null)
            {
                throw new Exception("The entity was not added.");
            }

            if (empl.Territories?.Count > 0)
            {
                TerritoryRepo.TryInsertMany(empl.Territories);
            }
        }
    }
}
