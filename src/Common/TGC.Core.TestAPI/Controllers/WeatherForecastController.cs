using Microsoft.AspNetCore.Mvc;
using TGC.Common.Exceptions;
using TGC.Common.Exceptions.Http;

namespace TGC.Core.TestAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger)
		{
			_logger = logger;
		}

		[HttpGet(Name = "GetWeatherForecast")]
		public IEnumerable<WeatherForecast> Get()
		{
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			})
			.ToArray();
		}

		[HttpGet("exception1")]
		public IEnumerable<WeatherForecast> Getexception1()
		{
			HttpExceptionFactory.ThrowIfNull<NotFoundException>(null, "Entity could not be found");
			return new List<WeatherForecast>();
		}

		[HttpGet("exception2")]
		public IEnumerable<WeatherForecast> Getexception2()
		{
            throw new NotFoundException();
        }
		
		[HttpGet("exception2")]
		public IActionResult Getexception4()
		{
			return Ok("sdasdasd");
		}
	}
}
