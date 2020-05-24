using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task.DB;
using Task.TestHelpers;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using Task.CustomSerializer;

namespace Task
{
	[TestClass]
	public class SerializationSolutions
	{
		Northwind dbContext;

		[TestInitialize]
		public void Initialize()
		{
			dbContext = new Northwind();
		}

		[TestMethod]
		public void SerializationCallbacks()
		{
			dbContext.Configuration.ProxyCreationEnabled = false;

			var tester = new XmlDataContractSerializerTester<IEnumerable<Category>>(new NetDataContractSerializer(
					new StreamingContext(StreamingContextStates.All, dbContext)), true);
			var categories = dbContext.Categories.ToList();

			var c = categories.First();

			tester.SerializeAndDeserialize(categories);
		}

		[TestMethod]
		public void ISerializable()
		{
			dbContext.Configuration.ProxyCreationEnabled = false;

			var tester = new XmlDataContractSerializerTester<IEnumerable<Product>>(new NetDataContractSerializer(
					new StreamingContext(StreamingContextStates.All, dbContext)), true); 
			var products = dbContext.Products.ToList();

			tester.SerializeAndDeserialize(products);
		}

		[TestMethod]
		public void ISerializationSurrogate()
		{
			dbContext.Configuration.ProxyCreationEnabled = false;

			var streamingContext = new StreamingContext(StreamingContextStates.All, dbContext);
			var surrogateSelector = new SurrogateSelector();

			surrogateSelector.AddSurrogate(
				typeof(Order_Detail),
				streamingContext, 
				new OrderDetailSerializerSurrogate());
			surrogateSelector.AddSurrogate(
				typeof(Product),
				streamingContext,
				new ProductSerializerSurrogate());

			var tester = new XmlDataContractSerializerTester<IEnumerable<Order_Detail>>(new NetDataContractSerializer(
					streamingContext,
					int.MaxValue,
					true,
					FormatterAssemblyStyle.Simple,
					surrogateSelector
				), true);

			var orderDetails = dbContext.Order_Details.ToList();
			tester.SerializeAndDeserialize(orderDetails);
		}

		[TestMethod]
		public void IDataContractSurrogate()
		{
			dbContext.Configuration.ProxyCreationEnabled = true;
			dbContext.Configuration.LazyLoadingEnabled = true;

			var tester = new XmlDataContractSerializerTester<IEnumerable<Order>>(
				new DataContractSerializer(typeof(IEnumerable<Order>), 
					new DataContractSerializerSettings()
					{
						PreserveObjectReferences = true,
						IgnoreExtensionDataObject = false,
						MaxItemsInObjectGraph = int.MaxValue,
						DataContractSurrogate = new OrderIDataContractSurrogate()
					}), true);

			var orders = dbContext.Orders.ToList();

			tester.SerializeAndDeserialize(orders);
		}
	}
}
