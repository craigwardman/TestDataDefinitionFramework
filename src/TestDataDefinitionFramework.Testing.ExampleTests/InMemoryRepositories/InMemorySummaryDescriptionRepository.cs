using System.Linq;
using System.Threading.Tasks;
using TestDataDefinitionFramework.Core;
using TestDataDefinitionFramework.Testing.ExampleSut.Abstractions;

namespace TestDataDefinitionFramework.Testing.ExampleTests.InMemoryRepositories
{
    public class InMemorySummaryDescriptionRepository : ISummaryDescriptionRepository
    {
        public Task<SummaryDescription?> GetSummaryDescription(string summaryName)
        {
            var result = TestDataStore.Repository<SummaryDescription>()
                .Items.FirstOrDefault(i => i.Name == summaryName);

            return Task.FromResult(result);
        }
    }
}