using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestDataDefinitionFramework.Core;
using TestDataDefinitionFramework.Testing.ExampleSut.Abstractions;
using TestDataDefinitionFramework.Testing.ExampleSut.MongoDB.Mongo;

namespace TestDataDefinitionFramework.Testing.ExampleTests.InMemoryRepositories
{
    public class InMemorySummariesRepository : ISummariesRepository
    {
        private readonly TestDataStore _testDataStore;

        public InMemorySummariesRepository(TestDataStore testDataStore)
        {
            _testDataStore = testDataStore;
        }

        public Task<IReadOnlyList<string>> GetAllAsync()
        {
            var result = _testDataStore.Repository<SummaryItem>().Items?.Select(i => i.Name).ToArray()
                ?? Array.Empty<string>();

            return Task.FromResult((IReadOnlyList<string>) result);
        }
    }
}