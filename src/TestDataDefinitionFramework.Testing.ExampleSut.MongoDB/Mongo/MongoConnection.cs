using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace TestDataDefinitionFramework.Testing.ExampleSut.MongoDB.Mongo
{
    internal class MongoConnection : IMongoConnection
    {
        public MongoConnection(IMongoDataStoreConfig config, IEnumerable<IBootstrapped> bootstrappedItems)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            var settings = MongoClientSettings.FromUrl(new MongoUrl(config.ConnectionString));
            var client = new MongoClient(settings);

            Database = client.GetDatabase(config.DatabaseName);

            foreach (var bootstrapped in bootstrappedItems ?? Array.Empty<IBootstrapped>())
            {
                bootstrapped.Setup(Database);
            }
        }

        public IMongoDatabase Database { get; }
    }
}