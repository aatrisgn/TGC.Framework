using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using TGC.WebApiBuilder;

namespace TGC.HealthChecks.IoC;
public class InstallHealthChecks : IEndpointRouteBuilderInstaller
{
	public void Install(IEndpointRouteBuilder webApplication)
	{
		webApplication.MapHealthChecks("/healthstatus", new HealthCheckOptions
		{
			ResponseWriter = HealthCheckHandler.WriteResponse
		});
	}
}
