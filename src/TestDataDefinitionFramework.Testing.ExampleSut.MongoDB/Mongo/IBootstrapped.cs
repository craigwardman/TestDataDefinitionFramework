using MongoDB.Driver;

namespace TestDataDefinitionFramework.Testing.ExampleSut.MongoDB.Mongo
{
    internal interface IBootstrapped
    {
        void Setup(IMongoDatabase database);
    }
}