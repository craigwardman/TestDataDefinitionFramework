using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace TestDataDefinitionFramework.Sql
{
    public static class SqlTableActions
    {
        public static async Task SetupSqlTable(SqlConnection connection, DataTable dataTable)
        {
            await using var command = connection.CreateCommand();

            await DropExistingTable(command, dataTable.TableName);
            await CreateSqlTable(dataTable, command);
        }

        private static async Task DropExistingTable(DbCommand command, string tableName)
        {
            command.CommandText = $"DROP TABLE IF EXISTS  {tableName}";
            await command.ExecuteNonQueryAsync();
        }

        private static async Task CreateSqlTable(DataTable dataTable, DbCommand command)
        {
            var commandTextBuilder = new StringBuilder();
            commandTextBuilder.AppendFormat("CREATE TABLE [{0}](", dataTable.TableName);

            var columnDefs = new string[dataTable.Columns.Count];
            for (var i = 0; i < dataTable.Columns.Count; i++)
            {
                var dataTableColumn = dataTable.Columns[i];
                columnDefs[i] =
                    $"[{dataTableColumn.ColumnName}] {dataTableColumn.DataType.ToSqlDbDefinition()} {(dataTableColumn.AllowDBNull ? "NULL" : "NOT NULL")}";
            }

            commandTextBuilder.AppendJoin(",", columnDefs);
            commandTextBuilder.Append(')');

            command.CommandText = commandTextBuilder.ToString();

            await command.ExecuteNonQueryAsync();
        }
    }
}