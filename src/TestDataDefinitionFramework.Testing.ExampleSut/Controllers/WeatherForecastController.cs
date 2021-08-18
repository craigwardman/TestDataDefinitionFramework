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

        public WeatherForecastController(ISummariesRepository summariesRepository)
        {
            _summariesRepository = summariesRepository ?? throw new ArgumentNullException(nameof(summariesRepository));
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var summaries = await _summariesRepository.GetAllAsync();
            var rng = new Random();
            return summaries.Select((summary, index) => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = summary
            })
            .ToArray();
        }
    }
}