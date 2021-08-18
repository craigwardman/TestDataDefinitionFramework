using System.Threading.Tasks;

namespace TestDataDefinitionFramework.Core
{
    public interface ITestDataBackingStore
    {
        Task CommitAsync(TestDataStore store);
    }
}