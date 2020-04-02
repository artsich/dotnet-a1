using Dapper;
using DapperExtensions;
using OrderManagement.DataAccess.Extensions;
using OrderManagement.DataAccess.Interfaces;
using OrderManagement.DataAccess.Models.Db;
using System;
using System.Collections.Generic;
using System.Data.Common;
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

        private const string Sql_TrySupplierInsert = @"
            declare @isExist as int
            set @isExist = (select count(SupplierID) from dbo.Suppliers where SupplierID=@SupplierID)
            if @isExist = 0
	            insert into dbo.Suppliers ([Address], City, CompanyName, ContactName, ContactTitle, Country, Fax, HomePage, Phone, PostalCode, Region)
	            values (@Address, @City, @CompanyName, @ContactName, @ContactTitle, @Country, @Fax, @HomePage, @Phone, @PostalCode, @Region);
            if @isExist = 0
	            select convert(int, IDENT_CURRENT('dbo.Suppliers'));";

        private const string Sql_TryCategoryInsert = @"
            declare @isExist as int
            set @isExist = (select count(CategoryID) from dbo.Categories where CategoryID=@CategoryID)
            if @isExist = 0
	            insert into dbo.Categories (CategoryName, [Description], Picture)
	            values (@CategoryName, @Description, @Picture);
            if @isExist = 0
	            select convert(int, IDENT_CURRENT('dbo.Categories'));";

        public ProductRepository(string connectinString, string provider)
            : base(connectinString, provider)
        {
        }

        public IList<Product> GetWholeProducts()
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

        public void InsertWhole(IList<Product> products)
        {
            void insertSuppliersAndSetId(IList<(Product p, Supplier s)> tuples, DbConnection conn, DbTransaction trans)
            {
                foreach(var (p, s) in tuples)
                {
                    var id = conn.ExecuteScalar<int?>(Sql_TrySupplierInsert, s, trans);
                    if (id.HasValue)
                    {
                        s.SupplierID = id.Value;
                        p.SupplierID = id.Value;
                    }
                }
            }

            void insertCustomersAndSetId(IList<(Product p, Category c)> tuples, DbConnection conn, DbTransaction trans)
            {
                foreach (var (p, c) in tuples)
                {
                    var id = conn.ExecuteScalar<int?>(Sql_TryCategoryInsert, c, trans);
                    if (id.HasValue)
                    {
                        c.CategoryID = id.Value;
                        p.CategoryID = id.Value;
                    }
                }
            }

            using (var connection = ProviderFactory.CreateConnection(ConnectionString))
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var suppliers = products
                            .Where(x => x.Supplier != null)
                            .Select(x => (x, x.Supplier)).ToList();
                        insertSuppliersAndSetId(suppliers, connection, transaction);

                        var categories = products
                            .Where(x => x.Category != null)
                            .Select(x => (x, x.Category)).ToList();
                        insertCustomersAndSetId(categories, connection, transaction);

                        foreach(var prod in products)
                        {
                            connection.Insert(prod, transaction);
                        }

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
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
