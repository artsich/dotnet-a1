using System.Data;
using System.Data.Common;

namespace OrderManagement.DataAccess.Extensions
{
    public static class DbCommandExtension
    {
        public static DbCommand AddParameter(this DbCommand command, string name, DbType type, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = type;
            parameter.Value = value;
            command.Parameters.Add(parameter);
            return command;
        }
    }
}