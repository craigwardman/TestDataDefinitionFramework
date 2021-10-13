using System;
using System.Collections.Generic;

namespace TestDataDefinitionFramework.Sql
{
    public static class SqlTypeLookup
    {
        // as per: https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings
        private static readonly Dictionary<Type, string> TypeMap = new()
        {
            { typeof(bool), "bit" },
            { typeof(byte), "tinyint" },
            { typeof(byte[]), "image" },
            { typeof(DateTime), "datetime" },
            { typeof(decimal), "decimal" },
            { typeof(double), "float" },
            { typeof(Guid), "uniqueidentifier" },
            { typeof(short), "smallint" },
            { typeof(int), "int" },
            { typeof(long), "bigint" },
            { typeof(object), "sql_variant" },
            { typeof(string), "varchar(255)" }
        };

        public static string ToSqlDbDefinition(this Type type) => TypeMap.TryGetValue(type, out var sqlDbType) ? sqlDbType : "sql_variant";
    }
}