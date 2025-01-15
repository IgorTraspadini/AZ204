using Microsoft.AspNetCore.Mvc;

namespace Tempo_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TempoForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<TempoForecastController> _logger;

        public WeatherForecastController(ILogger<TempoForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<TempoForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new TempoForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
