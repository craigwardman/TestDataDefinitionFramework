using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;
using TestDataDefinitionFramework.Core;
using TestDataDefinitionFramework.Testing.ExampleSut.MongoDB.Mongo;

namespace TestDataDefinitionFramework.Testing.ExampleTests.Features.WeatherForcecast
{
    [Binding]
    public class Steps
    {
        private readonly Interactions _interactions;
        private readonly Context _context;

        public Steps(Interactions interactions, Context context)
        {
            _interactions = interactions;
            _context = context;
        }

        [Given(@"the summaries repository returns no items")]
        public void GivenTheSummariesRepositoryReturnsNoItems()
        {
            TestDataStore.Repository<SummaryItem>(SummaryCollection.Name).Items = Array.Empty<SummaryItem>();
        }

        [When(@"a request is made to get the weather forecast")]
        public async Task WhenARequestIsMadeToGetTheWeatherForecast()
        {
            _context.WeatherForecastResult = await _interactions.GetWeatherForecastAsync();
        }

        [Then(@"the weather forecast contains '(.*)' items")]
        public void ThenTheWeatherForecastContainsItems(int expected)
        {
            _context.WeatherForecastResult
                .Should().HaveCount(expected);
        }

        [Given(@"the summaries repository returns '(.*)'")]
        public void GivenTheSummariesRepositoryReturns(IReadOnlyList<string> items)
        {
            TestDataStore.Repository<SummaryItem>(SummaryCollection.Name).Items = items.Select(i => new SummaryItem {Name = i}).ToArray();
        }

    }
}