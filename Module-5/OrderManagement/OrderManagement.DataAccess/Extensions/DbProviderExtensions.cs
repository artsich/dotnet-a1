using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.DataAccess.Extensions
{
    internal static class DbProviderFactoryExtensions
    {
        public static DbConnection CreateConnection(this DbProviderFactory factory, string connectionString)
        {
            var connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            return connection;
        }
    }
}
