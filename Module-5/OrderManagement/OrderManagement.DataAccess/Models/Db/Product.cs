using DapperExtensions.Mapper;

namespace OrderManagement.DataAccess.Models.Db
{
    public class Product
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public int? SupplierID { get; set; }

        public Supplier Supplier { get; set; }

        public int? CategoryID { get; set; }

        public Category Category { get; set; }

        public string QuantityPerUnit { get; set; }

        public decimal UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }
    }

    internal class ProductMapper : ClassMapper<Product>
    {
        public ProductMapper()
        {
            Table("[dbo].[Products]");
            AutoMap();
            UnMap(x => x.Supplier);
            UnMap(x => x.Category);
        }
    }
}