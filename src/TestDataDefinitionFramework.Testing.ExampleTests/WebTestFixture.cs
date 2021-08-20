﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TestDataDefinitionFramework.Testing.ExampleSut;
using TestDataDefinitionFramework.Testing.ExampleSut.Abstractions;
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
#endif
            });
        }
    }
}