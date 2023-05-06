using Microsoft.Extensions.DependencyInjection;
using TGC.HealthChecks;
using TGC.WebApiBuilder;

namespace TGC.OpenAPISpecGeneration.IoC;

[DependencyInjectionBuilder]
internal class DIBuilder : IDependencyInjecter
{
	public void InjectServices(IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<IApplicationBuilderInstaller, OpenAPIInstaller>();
		serviceCollection.AddSingleton<IHealthCheckExecutor, OpenAPIAvailableHealthCheck>();

		serviceCollection.AddEndpointsApiExplorer();
		serviceCollection.AddSwaggerGen();
		serviceCollection.ConfigureOptions<ConfigureSwaggerOptions>();
	}
}
