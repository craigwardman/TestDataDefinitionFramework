using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TestDataDefinitionFramework.Testing.ExampleSut;

namespace TestDataDefinitionFramework.Testing.ExampleTests.Features.WeatherForcecast
{
    public class Interactions
    {
        private readonly WebTestFixture _fixture;

        public Interactions(WebTestFixture fixture)
        {
            _fixture = fixture;
        }

        public HttpResponseMessage Response { get; private set; }

        public async Task<IReadOnlyList<WeatherForecast>> GetWeatherForecastAsync()
        {
            using var client = _fixture.CreateClient();

            Response = await client.GetAsync("weatherforecast");

            return Response.IsSuccessStatusCode
                ? await Response.Content.ReadAsAsync<WeatherForecast[]>()
                : null;
        }
    }
}