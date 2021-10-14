using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestDataDefinitionFramework.Core
{
    public interface ITestDataBackingStore
    {
        string ConnectionString { get; }

        Task InitializeAsync();

        Task CommitAsync<T>(RepositoryConfig config, IReadOnlyList<T> items);
    }
}