using Microsoft.Extensions.DependencyInjection;

namespace TestDataDefinitionFramework.Core
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTestDataDefinitionFramework(this IServiceCollection services, TestDataStore store)
        {
            services.AddSingleton(store);
        }
    }
}