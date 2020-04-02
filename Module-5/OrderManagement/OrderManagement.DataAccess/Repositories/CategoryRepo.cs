using OrderManagement.DataAccess.Interfaces;
using OrderManagement.DataAccess.Models.Db;

namespace OrderManagement.DataAccess.Repositories
{
    public class CategoryRepo : AbstractRepository<Category>, ICategoryRepo
    {
        public CategoryRepo(string connString, string providerName)
            : base(connString, providerName)
        {
        }
    }
}
