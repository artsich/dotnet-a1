﻿using Dapper;
using OrderManagement.DataAccess.Extensions;
using OrderManagement.DataAccess.Interfaces;
using OrderManagement.DataAccess.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderManagement.DataAccess.Repositories
{
    public class ProductRepository : AbstractRepository<Product>, IProductRepository
    {
        private const string Sql_GetAll = @"
                    SELECT TOP (1000) P.[ProductID]
                        ,P.[ProductName]
                        ,P.[SupplierID]
                        ,P.[CategoryID]
                        ,P.[QuantityPerUnit]
                        ,P.[UnitPrice]
                        ,P.[UnitsInStock]
                        ,P.[UnitsOnOrder]
                        ,P.[ReorderLevel]
                        ,P.[Discontinued]
	                    ,S.[SupplierID]
                        ,S.[CompanyName]
                        ,S.[ContactName]
                        ,S.[ContactTitle]
                        ,S.[Address]
                        ,S.[City]
                        ,S.[Region]
                        ,S.[PostalCode]
                        ,S.[Country]
                        ,S.[Phone]
                        ,S.[Fax]
                        ,S.[HomePage]
	                    ,C.[CategoryID]
                        ,C.[CategoryName]
                        ,C.[Description]
                        ,C.[Picture]
                    FROM [Northwind].[dbo].[Products] as P
                    inner join [dbo].Suppliers as S on S.SupplierID = P.SupplierID
                    inner join [dbo].Categories as C on C.CategoryID = P.CategoryID;";

        private const string Sql_MoveProductToAnotherCategory = @"
            update [dbo].Products
            set CategoryID = @toId
            where CategoryID = @fromId;";

        public ProductRepository(string connectinString, string provider)
            : base(connectinString, provider)
        {
        }

        public override IList<Product> GetAll()
        {
            using (var connection = ProviderFactory.CreateConnection(ConnectionString))
            {
                var result = connection.Query<Product, Supplier, Category, Product>(
                    Sql_GetAll,
                    (pr, sup, cat) =>
                    {
                        pr.Supplier = sup;
                        pr.Category = cat;
                        return pr;
                    },
                    splitOn: "ProductID,SupplierID,CategoryID").ToList();
                return result;
            }
        }

        public int MoveProductToAnotherCategory(int fromCategoryId, int toCategoryId)
        {
            using (var connectinon = ProviderFactory.CreateConnection(ConnectionString))
            {
                using (var transaction = connectinon.BeginTransaction())
                {
                    try
                    {
                        var rows = connectinon.Execute(
                            Sql_MoveProductToAnotherCategory,
                            new
                            {
                                @fromId = fromCategoryId,
                                @toId = toCategoryId
                            }, transaction: transaction);

                        transaction.Commit();
                        return rows;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }
    }
}
