using System.Threading.Tasks;

namespace TestDataDefinitionFramework.Testing.ExampleSut.Abstractions
{
    public interface ISummaryDescriptionRepository
    {
        Task<SummaryDescription> GetSummaryDescription(string summaryName);
    }
}