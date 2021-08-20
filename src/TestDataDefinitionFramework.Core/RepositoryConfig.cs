namespace TestDataDefinitionFramework.Core
{
    public class RepositoryConfig
    {
        public string Name { get; init; }

        public ITestDataBackingStore BackingStore { get; init; }
    }
}