using OrderManagement.DataAccess.Models.Db;

namespace OrderManagement.DataAccess.Contract.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        int MoveProductToAnotherCategory(int fromCategory, int toCategory);
    }
}
