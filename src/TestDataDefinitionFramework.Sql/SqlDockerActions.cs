using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using TestDataDefinitionFramework.Docker;

namespace TestDataDefinitionFramework.Sql
{
    public static class SqlDockerActions
    {
        private const string SqlSaPassword = "TestDataFramework2021";

        public static async Task<string> SetupDatabase(string databaseName)
        {
            await EnsureContainerRunning();
            await CreateDatabaseIfNotExists(databaseName);

            return GetDockerConnectionString(databaseName);
        }

        private static string GetDockerConnectionString(string databaseName)
        {
            return $"Server=localhost,1433; Database={databaseName}; User Id=sa; Password={SqlSaPassword}; MultipleActiveResultSets=True";
        }

        private static Task EnsureContainerRunning()
        {
            return DockerRunner.EnsureContainerIsRunningAsync("tddf-sql",
                "mcr.microsoft.com/mssql/server:2019-latest", 1433,
                new Dictionary<string, string>
                {
                    { "ACCEPT_EULA", "Y" },
                    { "SA_PASSWORD", SqlSaPassword }
                });
        }

        private static async Task CreateDatabaseIfNotExists(string databaseName)
        {
            var masterConnectionString = GetDockerConnectionString("master");
            var connection = await TryEstablishConnection(masterConnectionString);

            var command = connection.CreateCommand();
            command.CommandText = $@"
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{databaseName}')
BEGIN
CREATE DATABASE {databaseName}
END";

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();
            await connection.DisposeAsync();
        }

        private static async Task<SqlConnection> TryEstablishConnection(string connectionString)
        {
            for (var attempt = 1; attempt <= 10; attempt++)
            {
                var connection = new SqlConnection(connectionString);
                try
                {
                    await connection.OpenAsync();
                    return connection;
                }
                catch (SqlException)
                {
                    if (attempt == 10)
                        throw;
                    
                    await Task.Delay(1000 * attempt);
                }
            }

            throw new Exception("Unable to connect to SQL");
        }
    }
}