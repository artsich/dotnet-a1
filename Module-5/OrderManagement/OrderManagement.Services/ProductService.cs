using OrderManagement.DataAccess.Interfaces;
using OrderManagement.DataAccess.Models.Db;
using OrderManagement.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository ProductRepo;
        private readonly ISupplierRepo SupplierRepo;
        private readonly ICategoryRepo CategorytRepo;

        public ProductService(IProductRepository prodRepo, ISupplierRepo supRepo, ICategoryRepo catRepo)
        {
            ProductRepo = prodRepo;
            SupplierRepo = supRepo;
            CategorytRepo = catRepo;
        }

        public void Insert(Product entity)
        {
            throw new NotImplementedException();
        }

        public void InsertMany(IList<Product> products)
        {
            if (products != null || products.Count == 0)
            {
                throw new ArgumentException("The list with products null or empty.");
            }

            var suppliers = products
                .Where(x => x.Supplier != null)
                .Select(x => x.Supplier).ToList();

            var categories = products
                .Where(x => x.Category != null)
                .Select(x => x.Category).ToList();

            SupplierRepo.TryInsertMany(suppliers);
            CategorytRepo.TryInsertMany(categories);

            foreach(var prod in products)
            {
                if (ProductRepo.Get(prod.ProductID) != null)
                    throw new Exception($"The product with id: {prod.ProductID} already exists.");
            
                ProductRepo.Insert(prod);
            }
        }
    }
}
