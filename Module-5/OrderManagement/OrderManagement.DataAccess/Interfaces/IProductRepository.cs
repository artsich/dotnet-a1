using OrderManagement.DataAccess.Models.Db;
using System.Collections.Generic;

namespace OrderManagement.DataAccess.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        void InsertWhole(IList<Product> product);

        int MoveProductToAnotherCategory(int fromCategory, int toCategory);
    }
}
