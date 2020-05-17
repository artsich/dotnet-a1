using DapperExtensions.Mapper;

namespace OrderManagement.DataAccess.Models.Db
{
    public class OrderDetail
    {
        public int OrderId { get; set; }

        public Order Order { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public decimal UnitPrice { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }
    }

    internal class OrderDetailMapping : ClassMapper<OrderDetail>
    {
        public OrderDetailMapping()
        {
            Table("[dbo].[Order Details]");
            AutoMap();
            UnMap(x => x.Order);
            UnMap(x => x.Product);
        }
    }
}