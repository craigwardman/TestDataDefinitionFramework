using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using TestDataDefinitionFramework.Core;

namespace TestDataDefinitionFramework.Sql
{
    public sealed class SqlBackingStore : ITestDataBackingStore, IDisposable
    {
        private readonly string _databaseName;
        private SqlConnection _dbConnection;

        /// <param name="databaseName">Provide the database name you are using in your "real" repositories</param>
        /// <param name="connectionString">
        ///     Provide a connection string to your Sql instance. If you do not provide a connection
        ///     string then this class will attempt to spin up a Sql instance using your local Docker Desktop installation.
        /// </param>
        public SqlBackingStore(string databaseName, string connectionString = default)
        {
            if (string.IsNullOrWhiteSpace(databaseName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(databaseName));
            _databaseName = databaseName;
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; private set; }

        public async Task CommitAsync<T>(RepositoryConfig config, IReadOnlyList<T> items)
        {
            var dataTable = (items ?? Array.Empty<T>()).ToDataTable(config.Name);

            await SqlTableActions.SetupSqlTable(_dbConnection, dataTable);

            using var bulkCopy = new SqlBulkCopy(_dbConnection);
            bulkCopy.DestinationTableName = config.Name;
            await bulkCopy.WriteToServerAsync(dataTable);
        }

        public async Task InitializeAsync()
        {
            if (_dbConnection == null)
            {
                if (string.IsNullOrEmpty(ConnectionString))
                    ConnectionString = await SqlDockerActions.SetupDatabase(_databaseName);

                _dbConnection = new SqlConnection(ConnectionString);
                await _dbConnection.OpenAsync();
            }
        }

        public void Dispose()
        {
            _dbConnection?.Dispose();
        }
    }
}