using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TestDataDefinitionFramework.Core;
using TestDataDefinitionFramework.MongoDB;
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
            var mongoBackingStore = new MongoBackingStore("ExampleSutDB");
            TestDataStore.AddRepository<SummaryItem>(cfg =>
            {
                cfg.WithName(SummaryCollection.Name);
#if UseRealProvider
                    cfg.WithBackingStore(mongoBackingStore);
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