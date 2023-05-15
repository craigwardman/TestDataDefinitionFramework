namespace TestDataDefinitionFramework.Core
{
    public class RepositoryConfig
    {
        public string Name { get; init; } = string.Empty;

        public ITestDataBackingStore? BackingStore { get; init; }
    }
}