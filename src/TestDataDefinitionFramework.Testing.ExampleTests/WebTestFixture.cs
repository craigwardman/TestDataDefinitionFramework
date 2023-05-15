using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TestDataDefinitionFramework.Core;
using TestDataDefinitionFramework.Testing.ExampleSut;
using TestDataDefinitionFramework.Testing.ExampleSut.Abstractions;
using TestDataDefinitionFramework.Testing.ExampleSut.Infrastructure;
using TestDataDefinitionFramework.Testing.ExampleSut.MongoDB;
using TestDataDefinitionFramework.Testing.ExampleSut.MongoDB.Mongo;
using TestDataDefinitionFramework.Testing.ExampleSut.Sql;
using TestDataDefinitionFramework.Testing.ExampleTests.InMemoryRepositories;

namespace TestDataDefinitionFramework.Testing.ExampleTests
{
    public class WebTestFixture : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.UseEnvironment("Testing");

            builder.ConfigureTestServices(services =>
            {
#if !UseRealProvider
                services.AddTransient<ISummariesRepository, InMemorySummariesRepository>();
                services.AddTransient<ISummaryDescriptionRepository, InMemorySummaryDescriptionRepository>();
                services.AddTransient<ISummaryTemperatureRepository, InMemorySummaryTemperatureRepository>();
#else
                services.AddTransient<SqlDataStoreConfig>();
                services.AddTransient<ISqlDataStoreConfig>(sp =>
                {
                    var config = sp.GetRequiredService<SqlDataStoreConfig>();
                    config.ConnectionString = TestDataStore.Repository<SummaryDescription>().Config.BackingStore?
                        .ConnectionString ?? config.ConnectionString;
                    return config;
                });
#endif
            });
        }
    }
}