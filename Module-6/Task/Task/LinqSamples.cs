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
			var total = 40020;
			var query = dataSource.Customers.Where(x => x.Orders.Sum(o => o.Total) > total);
			Dump(query);
		}


		[Category("Restriction Operators")]
		[Title("Where - Task 2")]
		[Description("Для каждого клиента составьте список поставщиков, находящихся в той же стране и том же городе. Сделайте задания с использованием группировки и без")]
		public void Linq2()
		{
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

		private void Dump(IEnumerable<object> iter)
		{
			foreach (var i in iter)
			{
				ObjectDumper.Write(i);
			}
		}
	}
}
