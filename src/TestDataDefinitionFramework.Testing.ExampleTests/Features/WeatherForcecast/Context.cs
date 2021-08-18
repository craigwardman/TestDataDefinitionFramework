using System.Collections.Generic;
using TestDataDefinitionFramework.Testing.ExampleSut;

namespace TestDataDefinitionFramework.Testing.ExampleTests.Features.WeatherForcecast
{
    public class Context
    {
        public IReadOnlyList<WeatherForecast> WeatherForecastResult { get; set; }
    }
}