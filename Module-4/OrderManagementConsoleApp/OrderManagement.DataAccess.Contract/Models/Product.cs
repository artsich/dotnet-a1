using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.DataAccess.Contract.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        
        public string ProductName { get; set; }

        public int? SupplierId { get; set; }

        public Supplier Supplier { get; set; }
        
        public int? CategoryId { get; set; }

        public Category Category { get; set; }

        public string QuantityPerUnit { get; set; }

        public decimal UnitPrice { get; set; }

        public int? UnitsInStock { get; set; }

        public int? UnitsOnOrder { get; set; }

        public int? ReorderLevel { get; set; }

        public bool Discountined { get; set; }
    }
}
