using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TGC.WebApiBuilder;

namespace TGC.HealthChecks.IoC;
[DependencyInjectionBuilder]
internal class DIBuilder : IDependencyInjecter
{
	public void InjectServices(IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<IHealthCheck, HealthCheckHandler>();
		serviceCollection.AddScoped<IEndpointRouteBuilderInstaller, InstallHealthChecks>();
		serviceCollection.AddHealthChecks().AddCheck<HealthCheckHandler>("Handler");
	}
}
