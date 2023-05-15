using Microsoft.Extensions.Configuration;
using TestDataDefinitionFramework.Testing.ExampleSut.Redis;

namespace TestDataDefinitionFramework.Testing.ExampleSut.Infrastructure;

public class RedisDataStoreConfig : IRedisDataStoreConfig
{
    public RedisDataStoreConfig(IConfiguration configuration)
    {
        configuration.Bind("Redis", this);
    }
        
    public string ConnectionString { get; set; } = string.Empty;
}