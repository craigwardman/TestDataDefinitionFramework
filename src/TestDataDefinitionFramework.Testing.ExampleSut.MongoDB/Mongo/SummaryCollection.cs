using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace TestDataDefinitionFramework.Testing.ExampleSut.MongoDB.Mongo
{
    public class SummaryCollection : IBootstrapped
    {
        public const string Name = "summaries";

        public void Setup(IMongoDatabase database)
        {
            RegisterClassMaps();
            CreateCollections(database);
            CreateIndices(database);
        }

        private static void RegisterClassMaps()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(SummaryItem)))
            {
                BsonClassMap.RegisterClassMap<SummaryItem>(dataMap =>
                {
                    dataMap.AutoMap();
                    dataMap.MapIdField(m => m.Name);
                });
            }
        }

        private static void CreateIndices(IMongoDatabase database)
        {
            database.GetCollection<SummaryItem>(Name).Indexes.CreateOne(
                new CreateIndexModel<SummaryItem>(
                    Builders<SummaryItem>.IndexKeys
                        .Ascending(l => l.Name))
            );
        }

        private static void CreateCollections(IMongoDatabase database)
        {
            if (!database.ListCollectionNames().ToList().Contains(Name))
            {
                database.CreateCollection(Name);
            }
        }
    }
}