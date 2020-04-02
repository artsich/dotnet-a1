using OrderManagement.DataAccess.Interfaces;
using OrderManagement.DataAccess.Models.Db;
using System.Collections.Generic;

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
