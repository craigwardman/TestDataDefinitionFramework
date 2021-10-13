using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestDataDefinitionFramework.Testing.ExampleSut.Abstractions;

namespace TestDataDefinitionFramework.Testing.ExampleSut.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ISummariesRepository _summariesRepository;
        private readonly ISummaryDescriptionRepository _summaryDescriptionRepository;

        public WeatherForecastController(ISummariesRepository summariesRepository, ISummaryDescriptionRepository summaryDescriptionRepository)
        {
            _summariesRepository = summariesRepository ?? throw new ArgumentNullException(nameof(summariesRepository));
            _summaryDescriptionRepository = summaryDescriptionRepository ?? throw new ArgumentNullException(nameof(summaryDescriptionRepository));
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var summaries = await _summariesRepository.GetAllAsync();
            var rng = new Random();
            return await Task.WhenAll(summaries.Select(async (summary, index) => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = summary,
                Description = (await _summaryDescriptionRepository.GetSummaryDescription(summary))?.Description ?? string.Empty
            })
            .ToArray());
        }
    }
}