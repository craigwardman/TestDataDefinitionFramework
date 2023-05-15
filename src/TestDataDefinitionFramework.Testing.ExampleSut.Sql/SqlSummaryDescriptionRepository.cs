using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using TestDataDefinitionFramework.Testing.ExampleSut.Abstractions;

namespace TestDataDefinitionFramework.Testing.ExampleSut.Sql
{
    public sealed class SqlSummaryDescriptionRepository : ISummaryDescriptionRepository, IDisposable
    {
        private readonly IDbConnection _connection;

        public SqlSummaryDescriptionRepository(ISqlDataStoreConfig config)
        {
            _connection = new SqlConnection(config.ConnectionString);
            _connection.Open();
        }
        
        public async Task<SummaryDescription?> GetSummaryDescription(string summaryName)
        {
            var rows = await _connection.QueryAsync<SummaryDescription>(
                "SELECT * FROM SummaryDescription WHERE Name = @Name",
                new { Name = summaryName },
                null,
                null,
                CommandType.Text);

            return rows?.FirstOrDefault();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}