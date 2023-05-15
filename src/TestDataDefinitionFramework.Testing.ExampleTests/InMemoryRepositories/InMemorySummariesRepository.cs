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
        public Task<IReadOnlyList<string>> GetAllAsync()
        {
            var result = TestDataStore.Repository<SummaryItem>(SummaryCollection.Name)
                             .Items.Select(i => i.Name).ToArray();

            return Task.FromResult((IReadOnlyList<string>) result);
        }
    }
}