using OrderManagement.DataAccess.Models.Db;
using System.Collections.Generic;

namespace OrderManagement.DataAccess.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        int MoveProductToAnotherCategory(int fromCategory, int toCategory);

        IList<Product> GetWholeProducts();

        void InsertWhole(IList<Product> product);
    }
}
