using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace TestDataDefinitionFramework.Core
{
    public static class TestDataStore
    {
        private static readonly ConcurrentDictionary<string, RepositoryBase> 
            RepositoriesByName = new();

        public static void AddRepository<T>(Action<RepositoryConfigBuilder> configurationAction = null)
        {
            var repoConfigBuilder = new RepositoryConfigBuilder()
                .WithName(GetName<T>(string.Empty));
            
            configurationAction?.Invoke(repoConfigBuilder);

            var repoConfig = repoConfigBuilder.Build();

            if (!RepositoriesByName.TryAdd(repoConfig.Name, new Repository<T>(repoConfig)))
            {
                throw new InvalidOperationException(
                    $"Could not add repository with name {repoConfig.Name}. Names must be unique.");
            }
        }

        public static Repository<T> Repository<T>(string name = default)
        {
            if (!RepositoriesByName.TryGetValue(GetName<T>(name), out var repository))
            {
                throw new InvalidOperationException(
                    "Unknown repository, make sure you have registered it with the corresponding name using 'WithRepository'");
            }

            return (Repository<T>)repository;
        }

        private static string GetName<T>(string customName)
        {
            return !string.IsNullOrEmpty(customName) ?
                customName :
                typeof(T).FullName ?? throw new InvalidOperationException("Unsupported type name. You must supply a type that has a FullName, or give a custom name.");
        }

        public static Task InitializeAllAsync()
        {
            var allBackingStores = RepositoriesByName.Select(rn => rn.Value.Config.BackingStore)
                .Where(s => s != null)
                .GroupBy(s => s)
                .Select(s => s.Key)
                .ToArray();

            return Task.WhenAll(allBackingStores.Select(s => s.InitializeAsync()));
        }

        public static Task CommitAllAsync()
        {
            return Task.WhenAll(RepositoriesByName.Select(rn => rn.Value.CommitAsync()));
        }
    }
}