namespace OrderManagement.DataAccess.Interfaces
{
    public interface IOrderDetailRepo
    {
        bool ReplaceProduct(int orderId, int fromProductId, int toProductId);
    }
}
