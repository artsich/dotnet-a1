using System;
using System.Data.Common;

namespace OrderManagement.DataAccess
{
    public static class DbDataReaderExtension
    {
        public static T SafeCast<T>(this DbDataReader reader, int ordinal)
        {
            if (!reader.IsDBNull(ordinal))
            {
                return (T)reader.GetValue(ordinal);
            }
            return default;
        }

        public static decimal? SafeCastNullableDecimal(this DbDataReader reader, int ordinal)
        {
            return SafeCast<decimal?>(reader, ordinal);
        }

        public static int SafeCastInt32(this DbDataReader reader, int ordinal)
        {
            return SafeCast<int>(reader, ordinal);
        }

        public static int? SafeCastNullableInt32(this DbDataReader reader, int ordinal)
        {
            return SafeCast<int?>(reader, ordinal);
        }

        public static string SafeCastString(this DbDataReader reader, int ordinal)
        {
            return SafeCast<string>(reader, ordinal);
        }

        public static DateTime? SafeCastNullableDateTime(this DbDataReader reader, int ordinal)
        {
            return SafeCast<DateTime?>(reader, ordinal);
        }
    }
}