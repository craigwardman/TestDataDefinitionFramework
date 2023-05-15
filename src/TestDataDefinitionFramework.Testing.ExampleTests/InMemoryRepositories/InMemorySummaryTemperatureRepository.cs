using System.Linq;
using System.Threading.Tasks;
using TestDataDefinitionFramework.Core;
using TestDataDefinitionFramework.Testing.ExampleSut.Abstractions;

namespace TestDataDefinitionFramework.Testing.ExampleTests.InMemoryRepositories;

public class InMemorySummaryTemperatureRepository : ISummaryTemperatureRepository
{
    public Task<SummaryTemperature?> GetSummaryTemperature(string summaryName)
    {
        var result = TestDataStore.Repository<(string Key, SummaryTemperature? Value)>().Items.FirstOrDefault(i => i.Key == summaryName);

        return Task.FromResult(result.Value);
    }
}