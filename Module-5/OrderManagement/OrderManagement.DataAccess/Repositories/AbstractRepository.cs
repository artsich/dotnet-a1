using Dapper;
using DapperExtensions;
using OrderManagement.DataAccess.Extensions;
using OrderManagement.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace OrderManagement.DataAccess.Repositories
{
    public abstract class AbstractRepository<T> : IRepository<T>
        where T : class
    {
        protected readonly string ConnectionString;
        protected readonly string ProviderName;
        protected readonly DbProviderFactory ProviderFactory;

        protected virtual string Sql_TryInsertMany { get; }

        protected AbstractRepository(string connString, string providerName)
        {
            ConnectionString = connString;
            ProviderName = providerName;
            ProviderFactory = DbProviderFactories.GetFactory(providerName);
        }

        public virtual T Get(int id)
        {
            using (var connection = ProviderFactory.CreateConnection(ConnectionString))
            {
                return connection.Get<T>(id);
            }
        }

        public virtual IList<T> GetAll()
        {
            using (var connection = ProviderFactory.CreateConnection(ConnectionString))
            {
                return connection.GetList<T>().ToList();
            }
        }

        public virtual IList<T> GetWholeEntitiesList()
        {
            return GetAll();
        }

        public virtual bool Delete(T item)
        {
            using (var connection = ProviderFactory.CreateConnection(ConnectionString))
            {
                return connection.Delete(item);
            }
        }

        public virtual T Insert(T item)
        {
            using (var connection = ProviderFactory.CreateConnection(ConnectionString))
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Insert(item, transaction: transaction);
                        transaction.Commit();
                        return item;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public virtual void InsertMany(ICollection<T> entities)
        {
            using (var connection = ProviderFactory.CreateConnection(ConnectionString))
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Insert(entities, transaction);
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

        public virtual bool Update(T item)
        {
            using (var connection = ProviderFactory.CreateConnection(ConnectionString))
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var isUpdated = connection.Update(item, transaction: transaction);
                        transaction.Commit();
                        return isUpdated;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public virtual int TryInsertMany(ICollection<T> entities)
        {
            if (entities == null || entities.Count == 0)
                throw new ArgumentException("The list of entities null or empty.");

            if (string.IsNullOrEmpty(Sql_TryInsertMany))
                throw new Exception("Sql_InsertMany was not initialized.");

            using (var connection = ProviderFactory.CreateConnection(ConnectionString))
            {
                using (var transaction = connection.BeginTransaction())
                {
                    var resultAffectedRows = 0;
                    try
                    {
                        foreach (var entity in entities)
                        {
                            var affectedRows = connection.Execute(
                                Sql_TryInsertMany,
                                entity,
                                transaction);

                            resultAffectedRows += affectedRows > 0 ? affectedRows : 0;
                        }

                        transaction.Commit();
                        return resultAffectedRows;
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