using Microsoft.Extensions.Configuration;
using TestDataDefinitionFramework.Testing.ExampleSut.MongoDB;

namespace TestDataDefinitionFramework.Testing.ExampleSut.Infrastructure
{
    public class MongoDataStoreConfig : IMongoDataStoreConfig
    {
        public MongoDataStoreConfig(IConfiguration configuration)
        {
            configuration.Bind("MongoDB", this);
        }

        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }
}