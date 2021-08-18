using MongoDB.Driver;

namespace TestDataDefinitionFramework.Testing.ExampleSut.MongoDB.Mongo
{
    internal interface IMongoConnection
    {
        IMongoDatabase Database { get; }
    }
}