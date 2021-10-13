using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TestDataDefinitionFramework.Core;
using TestDataDefinitionFramework.MongoDB;
using TestDataDefinitionFramework.Sql;
using TestDataDefinitionFramework.Testing.ExampleSut.Abstractions;
using TestDataDefinitionFramework.Testing.ExampleSut.MongoDB.Mongo;

namespace TestDataDefinitionFramework.Testing.ExampleTests
{
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

            await TestDataStore.InitializeAllAsync();
        }

        [BeforeScenarioBlock]
        public async Task Commit()
        {
            if (_scenarioContext.CurrentScenarioBlock == ScenarioBlock.When)
            {
                await TestDataStore.CommitAllAsync();
            }
        }
    }
}