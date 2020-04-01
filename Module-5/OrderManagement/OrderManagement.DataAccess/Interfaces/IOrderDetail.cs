namespace OrderManagement.DataAccess.Contract.Interfaces
{
    public interface IOrderDetailRepo
    {
        bool ReplaceProduct(int orderId, int fromProductId, int toProductId);
    }
}
