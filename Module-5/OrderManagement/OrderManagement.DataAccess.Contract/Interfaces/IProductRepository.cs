using OrderManagement.DataAccess.Contract.Models;

namespace OrderManagement.DataAccess.Contract.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetProductWithCategoryAndSupplier();

    }
}
