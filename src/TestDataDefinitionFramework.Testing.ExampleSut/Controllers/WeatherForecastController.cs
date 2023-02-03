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
        private readonly ISummaryTemperatureRepository _summaryTemperatureRepository;

        public WeatherForecastController(ISummariesRepository summariesRepository, ISummaryDescriptionRepository summaryDescriptionRepository, ISummaryTemperatureRepository summaryTemperatureRepository)
        {
            _summariesRepository = summariesRepository ?? throw new ArgumentNullException(nameof(summariesRepository));
            _summaryDescriptionRepository = summaryDescriptionRepository ?? throw new ArgumentNullException(nameof(summaryDescriptionRepository));
            _summaryTemperatureRepository = summaryTemperatureRepository ?? throw new ArgumentNullException(nameof(summaryTemperatureRepository));
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var summaries = await _summariesRepository.GetAllAsync();

            return await Task.WhenAll(summaries.Select(async (summary, index) =>
                {
                    var temps = await _summaryTemperatureRepository.GetSummaryTemperature(summary);
                    return new WeatherForecast
                    {
                        Date = DateTime.Now.AddDays(index),
                        TempHigh = temps?.High ?? 0,
                        TempLow = temps?.Low ?? 0,
                        Summary = summary,
                        Description =
                            (await _summaryDescriptionRepository.GetSummaryDescription(summary))?.Description ??
                            string.Empty
                    };
                })
            .ToArray());
        }
    }
}