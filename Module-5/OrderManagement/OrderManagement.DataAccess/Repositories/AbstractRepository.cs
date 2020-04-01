using DapperExtensions;
using OrderManagement.DataAccess.Contract.Interfaces;
using OrderManagement.DataAccess.Extensions;
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

        protected AbstractRepository(string connString, string providerName)
        {
            ConnectionString = connString;
            ProviderName = providerName;
            ProviderFactory = DbProviderFactories.GetFactory(providerName);
        }

        public virtual bool Delete(T item)
        {
            using (var connection = ProviderFactory.CreateConnection(ConnectionString))
            {
                return connection.Delete(item);
            }
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

        public virtual T Insert(T item) 
        {
            var tp = DapperExtensions.DapperExtensions.DefaultMapper;

            using (var connection = ProviderFactory.CreateConnection(ConnectionString))
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Insert<T>(item, transaction: transaction);
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

        public virtual bool Update(T item)
        {
            using (var connection = ProviderFactory.CreateConnection(ConnectionString))
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var isUpdated = connection.Update<T>(item, transaction: transaction);
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
    }
}
