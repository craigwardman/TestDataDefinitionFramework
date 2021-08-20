using MongoDB.Driver;

namespace TestDataDefinitionFramework.MongoDB
{
    internal class MongoDatabaseFactory
    {
        public IMongoDatabase GetDatabase(string connectionString, string databaseName)
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            var client = new MongoClient(settings);

            return client.GetDatabase(databaseName);
        }
    }
}