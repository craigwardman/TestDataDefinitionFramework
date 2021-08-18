using System.Collections.Generic;

namespace TestDataDefinitionFramework.Core
{
    public class Repository<T>
    {
        public IReadOnlyList<T> Items { get; set; }
    }
}