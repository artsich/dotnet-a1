using OrderManagement.DataAccess.Models.Db;

namespace OrderManagement.DataAccess.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        int MoveProductToAnotherCategory(int fromCategory, int toCategory);
    }
}
