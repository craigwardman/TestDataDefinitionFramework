using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;
using TestDataDefinitionFramework.Core;
using TestDataDefinitionFramework.Docker;

namespace TestDataDefinitionFramework.Redis
{
    public class RedisBackingStore : ITestDataBackingStore
    {
        private IConnectionMultiplexer? _redisConnection;
        private readonly KeyValueResolver _keyValueResolver;

        /// <param name="keyValueResolver">You must provide an instance of the KeyValueResolver class that is configured to serialize keys and values (matching the method used by the SUT)</param>
        /// <param name="connectionString">
        ///     Provide a connection string to your Redis instance. If you do not provide a connection
        ///     string then this class will attempt to spin up a Redis instance using your local Docker Desktop installation.
        /// </param>
        public RedisBackingStore(KeyValueResolver keyValueResolver, string? connectionString = default)
        {
            _keyValueResolver = keyValueResolver ?? throw new ArgumentNullException(nameof(keyValueResolver));
            ConnectionString = connectionString ?? string.Empty;
        }

        public string ConnectionString { get; private set; }

        public async Task CommitAsync<T>(RepositoryConfig config, IReadOnlyList<T> items)
        {
            if (_redisConnection == null)
                throw new InvalidOperationException("You cannot call Commit until you've called Initialize");
            
            var db = _redisConnection.GetDatabase();

            if (items is { Count: > 0 })
            {
                foreach (var item in items)
                {
                    var keyAndValue = _keyValueResolver.GetKeyValue(item);
                    await db.StringSetAsync(keyAndValue.Key, keyAndValue.Value);
                }
            }
        }

        public async Task InitializeAsync()
        {
            if (_redisConnection == null)
            {
                if (string.IsNullOrEmpty(ConnectionString))
                {
                    await DockerRunner.EnsureContainerIsRunningAsync("tddf-redis", "redis:7.0.8-alpine", 6379);
                    ConnectionString = "localhost";
                }

                _redisConnection = await ConnectionMultiplexer.ConnectAsync(ConnectionString);
            }
        }
    }
}