using System.Threading.Tasks;

namespace TestDataDefinitionFramework.Testing.ExampleSut.Abstractions;

public interface ISummaryTemperatureRepository
{
    Task<SummaryTemperature> GetSummaryTemperature(string summaryName);
}