using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using TestDataDefinitionFramework.Testing.ExampleSut.Abstractions;
using TestDataDefinitionFramework.Testing.ExampleSut.MongoDB.Mongo;

namespace TestDataDefinitionFramework.Testing.ExampleSut.MongoDB
{
    internal class MongoSummariesRepository : ISummariesRepository
    {
        private readonly IMongoConnection _mongoConnection;

        public MongoSummariesRepository(IMongoConnection mongoConnection)
        {
            _mongoConnection = mongoConnection ?? throw new ArgumentNullException(nameof(mongoConnection));
        }

        public async Task<IReadOnlyList<string>> GetAllAsync()
        {
            var query = await _mongoConnection.Database.GetCollection<SummaryItem>(SummaryCollection.Name)
                .FindAsync(FilterDefinition<SummaryItem>.Empty);

            var results = await query.ToListAsync();

            return results?.Select(r => r.Name).ToArray() ?? Array.Empty<string>();
        }
    }
}