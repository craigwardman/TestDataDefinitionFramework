using Microsoft.Extensions.DependencyInjection;
using TestDataDefinitionFramework.Testing.ExampleSut.Abstractions;
using TestDataDefinitionFramework.Testing.ExampleSut.MongoDB.Mongo;

namespace TestDataDefinitionFramework.Testing.ExampleSut.MongoDB
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMongoRepository(this IServiceCollection services)
        {
            services.AddTransient<ISummariesRepository, MongoSummariesRepository>();

            services.AddSingleton<IMongoConnection, MongoConnection>();
            services.AddSingleton<IBootstrapped, SummaryCollection>();
        }
    }
}