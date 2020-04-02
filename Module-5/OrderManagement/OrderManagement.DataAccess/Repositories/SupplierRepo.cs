using OrderManagement.DataAccess.Interfaces;
using OrderManagement.DataAccess.Models.Db;

namespace OrderManagement.DataAccess.Repositories
{
    public class SupplierRepo : AbstractRepository<Supplier>, ISupplierRepo
    {
        public SupplierRepo(string connString, string providerName)
            : base(connString, providerName)
        {
        }
    }
}
