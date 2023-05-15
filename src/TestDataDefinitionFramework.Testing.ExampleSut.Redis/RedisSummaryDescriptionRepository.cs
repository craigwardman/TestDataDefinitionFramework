using System;
using System.Threading.Tasks;
using StackExchange.Redis;
using TestDataDefinitionFramework.Testing.ExampleSut.Abstractions;

namespace TestDataDefinitionFramework.Testing.ExampleSut.Redis
{
    public sealed class RedisTemperatureRepository : ISummaryTemperatureRepository, IDisposable
    {
        private readonly IRedisSerializer _redisSerializer;
        private readonly IConnectionMultiplexer _connection;

        public RedisTemperatureRepository(IRedisDataStoreConfig config, IRedisSerializer redisSerializer)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            
            _redisSerializer = redisSerializer ?? throw new ArgumentNullException(nameof(redisSerializer));
            _connection = ConnectionMultiplexer.Connect(config.ConnectionString);
        }
        
        public async Task<SummaryTemperature?> GetSummaryTemperature(string summaryName)
        {
            var redisValue = await _connection.GetDatabase().StringGetAsync(summaryName);
            return _redisSerializer.Deserialize<SummaryTemperature>(redisValue);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}