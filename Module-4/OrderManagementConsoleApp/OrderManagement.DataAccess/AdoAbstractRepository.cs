using OrderManagement.DataAccess.Contract;
using OrderManagement.DataAccess.Contract.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.DataAccess
{
    public abstract class AdoAbstractRepository<T> : IRepository<T>
        where T : BaseModel
    {
        protected readonly string ConnectionString;
        protected readonly DbProviderFactory ProviderFactory;

        protected string InsertSql;
        protected string GetByIdSql;
        protected string UpdateSql;
        protected string DeleteSql;
        protected string GetCollectionSql;

        protected string ParamIdName = "@id";

        public AdoAbstractRepository(string connectionString, string nameProvider)
        {
            ConnectionString = connectionString;
            ProviderFactory = DbProviderFactories.GetFactory(nameProvider);
        }

        protected abstract T FromReaderToObject(DbDataReader reader);

        public T GetBy(int guid)
        {
            T result = default;

            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = GetByIdSql;

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = ParamIdName;
                    parameter.DbType = DbType.Int32;
                    parameter.Value = guid;

                    command.Parameters.Add(parameter);
                    command.CommandType = CommandType.Text;

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

        public IList<T> GetCollection()
        {
            IList<T> result = new List<T>();

            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = GetCollectionSql;
                    command.CommandType = CommandType.Text;
                    
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

        public T Insert(T item)
        {
            throw new NotImplementedException();
        }

        public T Update(T  item)
        {
            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = UpdateSql;
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = ParamIdName;
                    parameter.DbType = DbType.Int32;
                    command.Parameters.Add(parameter);
                }
            }

            ///check this return statement
            return item;
        }
        
        public bool Delete(int id)
        {
            bool isDeleted = false;

            using (var connection = ProviderFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = DeleteSql;
                    var parameter = command.CreateParameter();
                    parameter.Value = id;
                    parameter.DbType = DbType.Int32;
                    command.Parameters.Add(parameter);
                    isDeleted = command.ExecuteNonQuery() > 0;
                }
            }

            return isDeleted;
        }
    }
}
