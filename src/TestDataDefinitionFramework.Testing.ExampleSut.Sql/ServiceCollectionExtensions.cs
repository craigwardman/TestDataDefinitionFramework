using Microsoft.Extensions.DependencyInjection;
using TestDataDefinitionFramework.Testing.ExampleSut.Abstractions;

namespace TestDataDefinitionFramework.Testing.ExampleSut.Sql
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSqlRepository(this IServiceCollection services)
        {
            services.AddScoped<ISummaryDescriptionRepository, SqlSummaryDescriptionRepository>();
        }
    }
}