using System;

namespace TestDataDefinitionFramework.Testing.ExampleSut
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TempHigh { get; set; }
        
        public int TempLow { get; set; }

        public string Summary { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
