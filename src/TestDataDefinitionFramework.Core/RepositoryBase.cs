using System;
using System.Threading.Tasks;

namespace TestDataDefinitionFramework.Core
{
    public abstract class RepositoryBase
    {
        protected RepositoryBase(RepositoryConfig config)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
        }

        internal RepositoryConfig Config { get; }

        internal abstract Task CommitAsync();
    }
}