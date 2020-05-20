namespace Task.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Runtime.Serialization;

    [Serializable]
    public partial class Product //: ISerializable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Order_Details = new HashSet<Order_Detail>();
        }

        public int ProductID { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }

        public int? SupplierID { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Detail> Order_Details { get; set; }

        public virtual Supplier Supplier { get; set; }

        private Product(SerializationInfo info, StreamingContext context)
        {
            ProductID = info.GetInt32(nameof(ProductID));
            ProductName = info.GetString(nameof(ProductName));
            SupplierID = info.GetInt32(nameof(SupplierID));
            QuantityPerUnit = info.GetString(nameof(QuantityPerUnit));
            UnitPrice = info.GetDecimal(nameof(UnitPrice));
            UnitsInStock = info.GetInt16(nameof(UnitsInStock));
            ReorderLevel = info.GetInt16(nameof(ReorderLevel));
            Discontinued = info.GetBoolean(nameof(Discontinued));
            Category = (Category)info.GetValue(nameof(Category), typeof(Category));
            Order_Details = (ICollection<Order_Detail>)info.GetValue(nameof(Order_Details), typeof(ICollection<Order_Detail>));
            Supplier = (Supplier)info.GetValue(nameof(Supplier), typeof(Supplier));

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (context.Context is Northwind dbcontext)
            {
                Order_Details = dbcontext.Order_Details.Where(x => x.ProductID == ProductID).ToList();
                Category = dbcontext.Categories.FirstOrDefault(x => x.CategoryID == CategoryID);
                Supplier = dbcontext.Suppliers.FirstOrDefault(x => x.SupplierID == SupplierID);

                info.AddValue(nameof(CategoryID), CategoryID);
                info.AddValue(nameof(ProductID), ProductID);
                info.AddValue(nameof(ProductName), ProductName);
                info.AddValue(nameof(SupplierID), SupplierID);
                info.AddValue(nameof(QuantityPerUnit), QuantityPerUnit);
                info.AddValue(nameof(UnitPrice), UnitPrice);
                info.AddValue(nameof(UnitsInStock), UnitsInStock);
                info.AddValue(nameof(ReorderLevel), ReorderLevel);
                info.AddValue(nameof(Discontinued), Discontinued);

                info.AddValue(nameof(Category), Category);
                info.AddValue(nameof(Order_Details), Order_Details);
                info.AddValue(nameof(Supplier), Supplier);
            }
        }
    }
}
