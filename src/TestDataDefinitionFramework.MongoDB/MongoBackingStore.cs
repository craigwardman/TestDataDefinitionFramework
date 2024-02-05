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
        private readonly string _databaseName;
        private IMongoDatabase? _mongoDatabase;

        /// <param name="databaseName">Provide the database name you are using in your "real" repositories</param>
        /// <param name="connectionString">
        ///     Provide a connection string to your MongoDB instance. If you do not provide a connection
        ///     string then this class will attempt to spin up a MongoDB instance using your local Docker Desktop installation.
        /// </param>
        public MongoBackingStore(string databaseName, string? connectionString = default)
        {
            if (string.IsNullOrWhiteSpace(databaseName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(databaseName));
            _databaseName = databaseName;
            ConnectionString = connectionString ?? string.Empty;
        }

        public string ConnectionString { get; private set; }

        public async Task CommitAsync<T>(RepositoryConfig config, IReadOnlyList<T> items)
        {
            if (_mongoDatabase == null)
                throw new InvalidOperationException("You cannot call Commit until you've called Initialize");
            
            await _mongoDatabase.DropCollectionAsync(config.Name);
            await _mongoDatabase.CreateCollectionAsync(config.Name);

            if (items is { Count: > 0 })
                await _mongoDatabase.GetCollection<T>(config.Name)
                    .InsertManyAsync(items);
        }

        public async Task InitializeAsync()
        {
            if (_mongoDatabase == null)
            {
                if (string.IsNullOrEmpty(ConnectionString))
                {
                    await DockerRunner.EnsureContainerIsRunningAsync("tddf-mongo", "mongo:latest", 27017);
                    ConnectionString = "mongodb://localhost:27017";
                }

                _mongoDatabase = new MongoDatabaseFactory().GetDatabase(ConnectionString, _databaseName);
            }
        }
    }
}