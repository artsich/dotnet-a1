// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using SampleSupport;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
	[Title("LINQ Module")]
	[Prefix("Linq")]
	public class LinqSamples : SampleHarness
	{
		private DataSource dataSource = new DataSource();

		[Category("Restriction Operators")]
		[Title("Sample Where - Task 1")]
		[Description("This sample uses the where clause to find all elements of an array with a value less than 5.")]
		public void LinqSampleOne()
		{
			int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

			var lowNums =
				from num in numbers
				where num < 5
				select num;

			Console.WriteLine("Numbers < 5:");
			foreach (var x in lowNums)
			{
				Console.WriteLine(x);
			}
		}

		[Category("Restriction Operators")]
		[Title("Sample Where - Task 2")]
		[Description("This sample return return all presented in market products")]

		public void LinqSampleTwo()
		{
			var products =
				from p in dataSource.Products
				where p.UnitsInStock > 0
				select p;

			foreach (var p in products)
			{
				ObjectDumper.Write(p);
			}
		}

		[Category("Restriction Operators")]
		[Title("Where - Task 1")]
		[Description("Выдайте список всех клиентов, чей суммарный оборот (сумма всех заказов) превосходит некоторую величину X. Продемонстрируйте выполнение запроса с различными X")]
		public void Linq1()
		{
			var query = Get_1(40041);
			Dump(query);

			query = Get_1(20041);
			Dump(query);
		}

		private IEnumerable<Customer> Get_1(int total)
		{
			return dataSource.Customers.Where(x => x.Orders.Sum(o => o.Total) > total);
		}

		[Category("Restriction Operators")]
		[Title("Where - Task 2 - w/o - group")]
		[Description("Для каждого клиента составьте список поставщиков, находящихся в той же стране и том же городе. Сделайте задания с использованием группировки и без")]
		public void Linq2()
		{
			var query = dataSource.Customers
					.Select(x => new
					{
						Customer = x,
						Suppliers = dataSource.Suppliers.Where(s => s.City == x.City && s.Country == x.Country).ToList()
					});

			Dump(query);
		}

		[Category("Restriction Operators")]
		[Title("Where - Task 2 - group")]
		[Description("Для каждого клиента составьте список поставщиков, находящихся в той же стране и том же городе.")]
		public void Linq2_group()
		{
			var query = dataSource.Customers.
				GroupJoin(dataSource.Suppliers,
						x => new { x.City, x.Country },
						s => new { s.City, s.Country }, (cus, supls) => new
						{
							Customer = cus,
							Supliers = supls.ToList()
						});

			Dump(query);
		}

		[Category("Restriction Operators")]
		[Title("Where - Task 3")]
		[Description("Найдите всех клиентов, у которых были заказы, превосходящие по сумме величину X")]
		public void Linq3()
		{
			var x = 400;
			var query = dataSource.Customers.Where(c => c.Orders.Any(o => o.Total > x));
			Dump(query);
		}

		[Category("Restriction Operators")]
		[Title("Where - Task 4")]
		[Description("Выдайте список клиентов с указанием, начиная с какого месяца какого года они стали клиентами (принять за таковые месяц и год самого первого заказа)")]
		public void Linq4()
		{
			var query = dataSource.Customers.Where(c => c.Orders?.Count() > 0)
				.Select(x => new
				{
					Customer = x,
					Date = x.Orders.OrderBy(o => o.OrderDate).First().OrderDate
				});

			Dump(query);
		}

		[Category("Restriction Operators")]
		[Title("Where - Task 5")]
		[Description("Сделайте предыдущее задание, но выдайте список отсортированным по году, месяцу, оборотам клиента (от максимального к минимальному) и имени клиента")]
		public void Linq5()
		{
			var query = dataSource.Customers
				.Where(c => c.Orders?.Count() > 0)
				.Select(x => new
				{
					Customer = x,
					Date = x.Orders.OrderBy(o => o.OrderDate).First().OrderDate
				})
				.OrderBy(x => x.Date.Year)
				.ThenBy(x => x.Date.Month)
				.ThenByDescending(x => x.Customer.Orders.Sum(o => o.Total))
				.ThenBy(x => x.Customer.CompanyName);

			//select() here because it more readable
			Dump(query.Select(x => new 
			{ 
				Name = x.Customer.CompanyName,
				x.Date,
				Total = x.Customer.Orders.Sum(o => o.Total)
			}));
		}

		[Category("Restriction Operators")]
		[Title("Where - Task 6")]
		[Description("Укажите всех клиентов, у которых указан нецифровой почтовый код или не заполнен регион или в телефоне не указан код оператора (для простоты считаем, что это равнозначно «нет круглых скобочек в начале")]
		public void Linq6()
		{
			var query = dataSource.Customers
				.Where(c => 
					!Regex.IsMatch(c.PostalCode ?? string.Empty, @"\d*") 
				||	string.IsNullOrEmpty(c.Region) 
				||	!c.Phone.StartsWith("("));

			Dump(query);
		}

		[Category("Restriction Operators")]
		[Title("Where - Task 7")]
		[Description("Сгруппируйте все продукты по категориям, внутри – по наличию на складе, внутри последней группы отсортируйте по стоимости")]
		public void Linq7()
		{
			var query = dataSource.Products
				.OrderBy(x => x.UnitsInStock)
				.ThenBy(x => x.UnitPrice)
				.GroupBy(x => x.Category);

			Dump(query);
		}

		public enum CostType 
		{
			Chipper,
			Average,
			Expensive
		}

		[Category("Restriction Operators")]
		[Title("Where - Task 8")]
		[Description("Сгруппируйте товары по группам «дешевые», «средняя цена», «дорогие». Границы каждой группы задайте сами")]
		public void Linq8()
		{
			var chipperIfLess = 5;
			var lessChipperIfLess = 15;

			var query = dataSource.Products
				.Select(p => new
				{
					Product = p,
					Type = p.UnitPrice < chipperIfLess ? CostType.Chipper :
							p.UnitPrice < lessChipperIfLess ? CostType.Average : CostType.Expensive
				})
				.GroupBy(x => x.Type);

			//select() here for beautify output.
			Dump(query
				.SelectMany(x => x.ToList())
				.Select(x => new 
				{ 
					x.Type, 
					x.Product.ProductName, 
					x.Product.UnitPrice 
				}));
		}

		[Category("Restriction Operators")]
		[Title("Where - Task9")]
		[Description("Рассчитайте среднюю прибыльность каждого города (среднюю сумму заказа по всем клиентам из данного города) и среднюю интенсивность (среднее количество заказов, приходящееся на клиента из каждого города)")]
		public void Linq9()
		{
			var query = dataSource.Customers
				.GroupBy(x => x.City)
				.Select(x => new
				{ 
					City = x.Key, 
					AverageRevenue = x.Average(c => c.Orders.Sum(o => o.Total)),
					AverageInensity = dataSource.Customers
													.Where(c => c.City == x.Key)
													.Average(c => c.Orders.Count())
				});

			Dump(query);
		}

		[Category("Restriction Operators")]
		[Title("Where - Task10")]
		[Description("Сделайте среднегодовую статистику активности клиентов по месяцам (без учета года), статистику по годам, по годам и месяцам (т.е. когда один месяц в разные годы имеет своё значение).")]
		public void Linq10()
		{
			var query = dataSource.Customers
				.Select(x => new
				{
					Customer = x,
					StatByMonth = x.Orders
									.GroupBy(o => o.OrderDate.Month)
									.Select(g => new { Month = g.Key, Count = g.Count() }),

					StatByYear = x.Orders
									.GroupBy(o => o.OrderDate.Year)
									.Select(g => new { Year = g.Key, Count = g.Count() }),

					StatByYearMonth = x.Orders
											.GroupBy(o => new 
											{ 
												o.OrderDate.Year,
												o.OrderDate.Month,
											})
											.Select(g => new
											{
												g.Key.Year,
												g.Key.Month,
												Count = g.Count()
											})
				});

			Dump(query);
		}

		private void Dump(IEnumerable<object> iter)
		{
			foreach (var i in iter)
			{
				ObjectDumper.Write(i);
			}
		}
	}
}
