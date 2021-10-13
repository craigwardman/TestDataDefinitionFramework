using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace TestDataDefinitionFramework.Sql
{
    public static class ItemsToDataTableExtension
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> self, string name)
        {
            var properties = typeof(T).GetProperties();

            var dataTable = new DataTable
            {
                TableName = name
            };
            foreach (var info in properties)
                dataTable.Columns.Add(info.Name, Nullable.GetUnderlyingType(info.PropertyType) 
                                                 ?? info.PropertyType);

            foreach (var entity in self)
                dataTable.Rows.Add(properties.Select(p => p.GetValue(entity)).ToArray());

            return dataTable;
        }     
    }
}