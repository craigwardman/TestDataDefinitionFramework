using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using TestDataDefinitionFramework.Core;
using TestDataDefinitionFramework.Docker;

namespace TestDataDefinitionFramework.MongoDB
{
    public class MongoBackingStore : ITestDataBackingStore
    {
        private IMongoDatabase _mongoDatabase;
        private readonly string _databaseName;
        private string _connectionString;

        /// <param name="databaseName">Provide the database name you are using in your "real" repositories</param>
        /// <param name="connectionString">Provide a connection string to your MongoDB instance. If you do not provide a connection string then this class will attempt to spin up a MongoDB instance using your local Docker Desktop installation.</param>
        public MongoBackingStore(string databaseName, string connectionString = default)
        {
            if (string.IsNullOrWhiteSpace(databaseName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(databaseName));
            _databaseName = databaseName;
            _connectionString = connectionString;
        }

        public async Task CommitAsync<T>(RepositoryConfig config, IReadOnlyList<T> items)
        {
            await _mongoDatabase.DropCollectionAsync(config.Name);
            await _mongoDatabase.CreateCollectionAsync(config.Name);

            if (items is {Count: > 0})
            {
                await _mongoDatabase.GetCollection<T>(config.Name)
                    .InsertManyAsync(items);
            }
        }

        public async Task InitializeAsync()
        {
            if (_mongoDatabase == null)
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    await DockerRunner.EnsureContainerIsRunningAsync("tddf-mongo", "mongo:4.0-xenial", 27017);
                    _connectionString = "mongodb://localhost:27017";
                }

                _mongoDatabase = new MongoDatabaseFactory().GetDatabase(_connectionString, _databaseName);
            }
        }
    }
}