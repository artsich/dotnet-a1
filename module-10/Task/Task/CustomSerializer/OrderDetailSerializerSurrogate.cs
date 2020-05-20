using System.Linq;
using System.Runtime.Serialization;
using Task.DB;

namespace Task.CustomSerializer
{
	public class OrderDetailSerializerSurrogate : ISerializationSurrogate
	{
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			var dbcontext = (Northwind)context.Context;
			var orderDetail = (Order_Detail)obj;
			var order = dbcontext.Orders.FirstOrDefault(x => x.OrderID == orderDetail.OrderID);
			var product = dbcontext.Products.FirstOrDefault(x => x.ProductID == orderDetail.ProductID);


			info.AddValue(nameof(orderDetail.OrderID), orderDetail.OrderID);
			info.AddValue(nameof(orderDetail.ProductID), orderDetail.ProductID);
			info.AddValue(nameof(orderDetail.UnitPrice), orderDetail.UnitPrice);
			info.AddValue(nameof(orderDetail.Quantity), orderDetail.Quantity);
			info.AddValue(nameof(orderDetail.Discount), orderDetail.Discount);
			info.AddValue(nameof(orderDetail.Order), order);
			info.AddValue(nameof(orderDetail.Product), product);
		}

		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			var orderDetail = (Order_Detail)obj;

			orderDetail.OrderID = info.GetInt32(nameof(orderDetail.OrderID));
			orderDetail.ProductID = info.GetInt32(nameof(orderDetail.ProductID));
			orderDetail.UnitPrice = info.GetDecimal(nameof(orderDetail.UnitPrice));
			orderDetail.Quantity = info.GetInt16(nameof(orderDetail.Quantity));
			orderDetail.Discount = (float)info.GetDouble(nameof(orderDetail.Discount));

			orderDetail.Order = (Order)info.GetValue(nameof(orderDetail.Order), typeof(Order));
			orderDetail.Product = (Product)info.GetValue(nameof(orderDetail.Product), typeof(Product));
			return obj;
		}
	}

	public class ProductSerializerSurrogate : ISerializationSurrogate
	{
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			var product = (Product)obj;
			info.AddValue(nameof(product.CategoryID), product.CategoryID);
			info.AddValue(nameof(product.ProductID), product.ProductID);
			info.AddValue(nameof(product.ProductName), product.ProductName);
			info.AddValue(nameof(product.SupplierID), product.SupplierID);
			info.AddValue(nameof(product.QuantityPerUnit), product.QuantityPerUnit);
			info.AddValue(nameof(product.UnitPrice), product.UnitPrice);
			info.AddValue(nameof(product.UnitsInStock), product.UnitsInStock);
			info.AddValue(nameof(product.ReorderLevel), product.ReorderLevel);
			info.AddValue(nameof(product.Discontinued), product.Discontinued);
		}

		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			var product = (Product)obj;
			product.ProductID = info.GetInt32(nameof(product.ProductID));
			product.ProductName = info.GetString(nameof(product.ProductName));
			product.SupplierID = info.GetInt32(nameof(product.SupplierID));
			product.QuantityPerUnit = info.GetString(nameof(product.QuantityPerUnit));
			product.UnitPrice = info.GetDecimal(nameof(product.UnitPrice));
			product.UnitsInStock = info.GetInt16(nameof(product.UnitsInStock));
			product.ReorderLevel = info.GetInt16(nameof(product.ReorderLevel));
			product.Discontinued = info.GetBoolean(nameof(product.Discontinued));
			return obj;
		}
	}
}
