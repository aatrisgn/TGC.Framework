using Microsoft.AspNetCore.Mvc;
namespace WebApplication1.Test.Controllers.V2;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("2.0")]
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

	[MapToApiVersion("2.0")]
	[HttpGet]
	public string Get()
	{
		return "Some vale";
	}
}