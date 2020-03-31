using OrderManagement.DataAccess.Contract;
using OrderManagement.DataAccess.Contract.Models;
using OrderManagement.DataAccess.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace OrderManagement.DataAccess
{
    public abstract class AdoAbstractRepository<T> : IRepository<T>
    {
        protected const string ParamIdName = "@id";

        protected readonly string ConnectionString;
        protected readonly DbProviderFactory ProviderFactory;

        protected abstract string GetByIdSql { get; }

        protected abstract string DeleteSql { get; }

        protected abstract string GetCollectionSql { get; }

        public AdoAbstractRepository(string connectionString, string nameProvider)
        {
            ConnectionString = connectionString;
            ProviderFactory = DbProviderFactories.GetFactory(nameProvider);
        }

        protected abstract T FromReaderToObject(DbDataReader reader);

        public abstract T Insert(T item);

        public abstract void Update(T item);

        public virtual T GetBy(int guid)
        {
            T result = default;

            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = GetByIdSql;
                    command.AddParameter(ParamIdName, DbType.Int32, guid);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = FromReaderToObject(reader);
                        }
                    }
                }
            }

            return result;
        }

        public virtual IList<T> GetCollection()
        {
            IList<T> result = new List<T>();

            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = GetCollectionSql;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(FromReaderToObject(reader));
                        }
                    }
                }
            }

            return result;
        }

        public virtual bool Delete(int id)
        {
            bool isDeleted = false;

            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = DeleteSql;
                    command.AddParameter(ParamIdName, DbType.Int32, id);
                    isDeleted = command.ExecuteNonQuery() > 0;
                }
            }

            return isDeleted;
        }
    }
}
