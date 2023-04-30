using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TGC.WebApiBuilder;

namespace TGC.HealthChecks.IoC;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddHealthCheck(this IServiceCollection services)
	{
		var healthChecks = services.Where(x => x.ServiceType == typeof(IHealthCheck)).ToList();

		var healthCheckService = services.AddHealthChecks();

		foreach (var healthCheck in healthChecks)
		{
			if (healthCheck.ImplementationType is IHealthCheck)
			{
				var implementationType = (IHealthCheck)healthCheck.ImplementationType;

				healthCheckService.AddCheck(nameof(implementationType), implementationType);
			}
		}

		services.AddScoped<IApplicationBuilderInstaller, InstallHealthChecks>();
		return services;
	}
}
