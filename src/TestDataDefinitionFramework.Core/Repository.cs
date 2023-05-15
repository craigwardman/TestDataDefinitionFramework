using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestDataDefinitionFramework.Core
{
    public class Repository<T> : RepositoryBase
    {
        public Repository(RepositoryConfig config)
        : base(config)
        {
        }

        public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();

        internal override Task CommitAsync()
        {
            return Config.BackingStore == null ?
                Task.CompletedTask :
                Config.BackingStore.CommitAsync(Config, Items);
        }
    }
}