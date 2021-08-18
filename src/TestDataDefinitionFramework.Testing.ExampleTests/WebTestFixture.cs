using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TestDataDefinitionFramework.Core;
using TestDataDefinitionFramework.Testing.ExampleSut;
using TestDataDefinitionFramework.Testing.ExampleSut.Abstractions;
using TestDataDefinitionFramework.Testing.ExampleTests.InMemoryRepositories;

namespace TestDataDefinitionFramework.Testing.ExampleTests
{
    public class WebTestFixture : WebApplicationFactory<Startup>
    {
        private readonly TestDataStore _store;

        public WebTestFixture(TestDataStore store)
        {
            _store = store;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.UseEnvironment("Testing");

            builder.ConfigureTestServices(services =>
            {
#if DEBUG
                services.AddTransient<ISummariesRepository, InMemorySummariesRepository>();
#else
                _store.UseBackingStore(new MongoDbBackingStore());
#endif

                services.AddTestDataDefinitionFramework(_store);
            });
        }
    }
}