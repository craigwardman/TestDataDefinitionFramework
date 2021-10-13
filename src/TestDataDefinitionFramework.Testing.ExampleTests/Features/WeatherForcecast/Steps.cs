using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TestDataDefinitionFramework.Core;
using TestDataDefinitionFramework.Testing.ExampleSut;
using TestDataDefinitionFramework.Testing.ExampleSut.Abstractions;
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

        [Given(@"the summary description repository returns")]
        public void GivenTheSummaryDescriptionRepositoryReturns(Table table)
        {
            var items = table.CreateSet<SummaryDescription>().ToArray();
            TestDataStore.Repository<SummaryDescription>().Items = items;
        }

        [Then(@"the weather forecase items match the repository data")]
        public void ThenTheWeatherForecaseItemsMatchTheRepositoryData()
        {
            var repositorySummaryItems = TestDataStore.Repository<SummaryItem>(SummaryCollection.Name).Items;
            var repositoryDescriptionItems = TestDataStore.Repository<SummaryDescription>().Items;

            var expected = repositorySummaryItems.Select(rs =>
                new WeatherForecast
                {
                    Summary = rs.Name,
                    Description = repositoryDescriptionItems.Single(d => d.Name == rs.Name).Description
                }).ToList();

            _context.WeatherForecastResult.Should().BeEquivalentTo(expected,
                cfg => cfg.Including(wf => wf.Summary).Including(wf => wf.Description));
        }
    }
}