using Microsoft.Extensions.DependencyInjection;
using TestDataDefinitionFramework.Testing.ExampleSut.Abstractions;

namespace TestDataDefinitionFramework.Testing.ExampleSut.Redis
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRedisRepository(this IServiceCollection services)
        {
            services.AddScoped<ISummaryTemperatureRepository, RedisTemperatureRepository>();
            services.AddTransient<IRedisSerializer, JsonRedisSerializer>();
        }
    }
}