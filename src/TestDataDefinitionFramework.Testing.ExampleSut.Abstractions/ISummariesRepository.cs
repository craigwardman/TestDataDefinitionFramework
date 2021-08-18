using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestDataDefinitionFramework.Testing.ExampleSut.Abstractions
{
    public interface ISummariesRepository
    {
        Task<IReadOnlyList<string>> GetAllAsync();
    }
}