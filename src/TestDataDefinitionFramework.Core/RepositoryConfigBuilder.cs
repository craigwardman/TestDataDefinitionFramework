using System;

namespace TestDataDefinitionFramework.Core
{
    public class RepositoryConfigBuilder
    {
        private ITestDataBackingStore _backingStore;
        private string _name;

        public RepositoryConfigBuilder WithBackingStore(ITestDataBackingStore backingStore)
        {
            _backingStore = backingStore;
            return this;
        }

        public RepositoryConfigBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public RepositoryConfig Build()
        {
            if (string.IsNullOrEmpty(_name))
            {
                throw new InvalidOperationException("Repository must have a name");
            }

            return new RepositoryConfig
            {
                BackingStore = _backingStore,
                Name = _name
            };
        }
    }
}