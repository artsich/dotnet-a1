using OrderManagement.DataAccess.Contract.Infrastructure;

namespace OrderManagement.DataAccess.Contract.Models
{
    public class OrderDetail
    {
        public int OrderId{ get; set; }

        [OptionalField]
        public Order Order { get; set; }

        public int ProductId { get; set; }

        [OptionalField]
        public Product Product { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public double Discount { get; set; }
    }
}