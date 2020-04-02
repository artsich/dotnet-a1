using Dapper;
using OrderManagement.DataAccess.Extensions;
using OrderManagement.DataAccess.Interfaces;
using OrderManagement.DataAccess.Models.Db;
using System;
using System.Collections.Generic;

namespace OrderManagement.DataAccess.Repositories
{
    public class TerritoryRepo : AbstractRepository<Territory>, ITerritoryRepo
    {
        private const string Sql_TryAddTerritories = @"
            if not exists (select * from dbo.Territories where TerritoryID=@id)
	            insert into dbo.Territories (TerritoryID, TerritoryDescription, RegionID)
	            values (@id, @desc, @regId);";

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
                    var resultAffectedRows = 0;
                    try
                    {
                        foreach(var entity in entities)
                        {
                            var affectedRows = connection.Execute(
                                Sql_TryAddTerritories, 
                                new
                                {
                                    @id = entity.TerritoryID,
                                    @desc = entity.TerritoryDescription,
                                    @regId = entity.RegionId
                                }, 
                                transaction: transaction);

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
