using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace TestDataDefinitionFramework.Core
{
    public class TestDataStore
    {
        private readonly ConcurrentDictionary<string, object> _repositories = new();

        internal ITestDataBackingStore BackingStore { get; private set; }

        public Repository<T> Repository<T>(string name = default)
        {
            name = !string.IsNullOrEmpty(name) ? name : typeof(T).FullName;

            return (Repository<T>)_repositories.GetOrAdd(name, (_) => new Repository<T>());
        }

        public TestDataStore UseBackingStore<T>(T backingStore) where T : ITestDataBackingStore
        {
            BackingStore = backingStore;
            return this;
        }

        internal Task CommitAsync()
        {
            return BackingStore?.CommitAsync(this) ?? Task.CompletedTask;
        }
    }
}