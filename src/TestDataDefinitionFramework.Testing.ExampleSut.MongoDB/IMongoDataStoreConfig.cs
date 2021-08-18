namespace TestDataDefinitionFramework.Testing.ExampleSut.MongoDB
{
    public interface IMongoDataStoreConfig
    {
        string ConnectionString { get; }
        string DatabaseName { get; }
    }
}