using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TestDataDefinitionFramework.Core;
using TestDataDefinitionFramework.MongoDB;
using TestDataDefinitionFramework.Redis;
using TestDataDefinitionFramework.Sql;
using TestDataDefinitionFramework.Testing.ExampleSut.Abstractions;
using TestDataDefinitionFramework.Testing.ExampleSut.MongoDB.Mongo;
using TestDataDefinitionFramework.Testing.ExampleSut.Redis;

namespace TestDataDefinitionFramework.Testing.ExampleTests;

[Binding]
public class TestDataSourceHooks
{
    private readonly ScenarioContext _scenarioContext;

    public TestDataSourceHooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeTestRun]
    public static async Task Initialize()
    {
#if UseRealProvider
        var mongoBackingStore = new MongoBackingStore("ExampleSutDB");
        var sqlBackingStore = new SqlBackingStore("SummaryDescriptions");

        var sutRedisSerializer = new JsonRedisSerializer(); // match the SUT
        var redisBackingStore = new RedisBackingStore(
            new KeyValueResolver()
                .WithResolver<(string Key, SummaryTemperature Value)>(
                    item => (item.Key, sutRedisSerializer.Serialize(item.Value))
                ));
#endif
        
        TestDataStore.AddRepository<SummaryItem>(cfg =>
        {
            cfg.WithName(SummaryCollection.Name);
#if UseRealProvider
            cfg.WithBackingStore(mongoBackingStore);
#endif
        });

        TestDataStore.AddRepository<SummaryDescription>(cfg =>
        {
#if UseRealProvider
            cfg.WithBackingStore(sqlBackingStore);
#endif
        });

        // SummaryTemperature has no natural key, so it's wrapped in a tuple to be treated as a key/value store
        TestDataStore.AddRepository<(string Key, SummaryTemperature Value)>(cfg =>
        {
#if UseRealProvider
            cfg.WithBackingStore(redisBackingStore);
#endif
        });

        await TestDataStore.InitializeAllAsync();
    }

    [BeforeScenarioBlock]
    public async Task Commit()
    {
        if (_scenarioContext.CurrentScenarioBlock == ScenarioBlock.When) await TestDataStore.CommitAllAsync();
    }
}