using Dapper;
using OrderManagement.DataAccess.Extensions;
using OrderManagement.DataAccess.Interfaces;
using OrderManagement.DataAccess.Models.Db;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.DataAccess.Repositories
{
    public class TerritoryRepo : AbstractRepository<Territory>, ITerritoryRepo
    {
        public TerritoryRepo(string connString, string providerName)
            : base(connString, providerName)
        {
        }

        public override int TryInsertMany(ICollection<Territory> entities)
        {
            if (entities == null || entities.Count == 0)
                throw new ArgumentException("The list of entities null or empty.");

            using (var connection = ProviderFactory.CreateConnection(ConnectionString))
            {
                using (var transaction = connection.BeginTransaction())
                {
                    var affectedRows = 0;
                    try
                    {
                        var queryBuilder = new StringBuilder();
                        var command = connection.CreateCommand();

                        for (var i = 0; i < entities.Count; ++i)
                        {
                            queryBuilder.AppendLine($@"
                                if not exists(select * from dbo.Territories where TerritoryID = @id_{i})
                                    insert into dbo.Territories(TerritoryID, TerritoryDescription, RegionID)
                                    values(@id_{i}, @desc_{i}, @regId_{i});");

                            command.AddParameter($"@id_{i}", System.Data.DbType.String, entities.ElementAt(i).TerritoryID);
                            command.AddParameter($"@desc_{i}", System.Data.DbType.String, entities.ElementAt(i).TerritoryDescription);
                            command.AddParameter($"@regId_{i}", System.Data.DbType.Int32, entities.ElementAt(i).RegionId);
                        }
                        command.CommandText = queryBuilder.ToString();
                        command.Transaction = transaction;
                        affectedRows = command.ExecuteNonQuery();
                        transaction.Commit();

                        return affectedRows < 0 ? 0 : affectedRows;
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
