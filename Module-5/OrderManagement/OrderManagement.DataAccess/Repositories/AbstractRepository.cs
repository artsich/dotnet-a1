using Dapper;
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

        public virtual void Delete(T item)
        {

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
            using (var connection = ProviderFactory.CreateConnection(ConnectionString))
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int id = connection.Insert<T>(item);
                        transaction.Commit();
                        throw new NotImplementedException();
                        return item;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return null;
                    }
                }
            }
        }

        public virtual void Update(T item)
        {
            using (var connection = ProviderFactory.CreateConnection(ConnectionString))
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result = connection.Update<T>(item);
                        transaction.Commit();
                        throw new NotImplementedException();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
