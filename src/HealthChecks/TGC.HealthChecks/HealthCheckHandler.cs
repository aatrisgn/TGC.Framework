using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TGC.HealthChecks;
internal class HealthCheckHandler : IHealthCheck
{
	private readonly IEnumerable<IHealthCheckExecutor> _healthCheckExecutors;
	public HealthCheckHandler(IEnumerable<IHealthCheckExecutor> healthCheckExecutors)
	{
		_healthCheckExecutors = healthCheckExecutors;
	}

	public async Task<HealthCheckResult> CheckHealthAsync(
		HealthCheckContext context, CancellationToken cancellationToken = default)
	{
		var isHealthy = true;
		var healthCheckResults = new List<HealthCheckExecutionResult>();

		foreach (var healthCheck in _healthCheckExecutors)
		{
			var healthCheckResult = await healthCheck.CheckHealth();

			if (healthCheckResult.HealthStatus == HealthCheckExecutionStatusEnum.Unhealthy)
			{
				isHealthy = false;
			}
			healthCheckResults.Add(healthCheckResult);
		}

		var healthCheckSerialized = JsonSerializer.Serialize(healthCheckResults);

		if (isHealthy)
		{
			return new HealthCheckResult(HealthStatus.Healthy, healthCheckSerialized);
		}

		return new HealthCheckResult(context.Registration.FailureStatus, healthCheckSerialized);
	}

	internal static Task WriteResponse(HttpContext context, HealthReport healthReport)
	{
		context.Response.ContentType = "application/json; charset=utf-8";
		var healthCheckDescriptions = healthReport.Entries.FirstOrDefault().Value.Description;

		if (string.IsNullOrEmpty(healthCheckDescriptions) == false)
		{
			return context.Response.WriteAsync(healthCheckDescriptions);
		}

		throw new NullReferenceException("No description for health checks was generated");
	}
}
