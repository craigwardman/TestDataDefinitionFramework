using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace TestDataDefinitionFramework.Core.SpecFlow
{
    [Binding]
    public class InitializeTestDataSourceHook
    {
        private readonly TestDataStore _store;

        public InitializeTestDataSourceHook(TestDataStore store)
        {
            _store = store;
        }

        [BeforeScenarioBlock("When")]
        public async Task Commit()
        {
            await _store.CommitAsync();
        }
    }
}