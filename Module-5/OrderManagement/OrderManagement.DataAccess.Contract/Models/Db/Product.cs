using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.DataAccess.Contract.Models.Db
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        public string ProductName { get; set; }

        [ForeignKey("SupplierID")]
        public int? SupplierID { get; set; }

        public Supplier Supplier { get; set; }

        [ForeignKey("CategoryID")]
        public int? CategoryID { get; set; }

        public Category Category { get; set; }

        public string QuantityPerUnit { get; set; }

        public decimal UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discountined { get; set; }
    }
}