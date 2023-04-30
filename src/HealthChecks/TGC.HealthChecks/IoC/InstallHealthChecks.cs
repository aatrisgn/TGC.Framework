using Microsoft.AspNetCore.Builder;
using TGC.WebApiBuilder;

namespace TGC.HealthChecks.IoC;
public class InstallHealthChecks : IApplicationBuilderInstaller
{
	public void Install(WebApplication webApplication)
	{
		webApplication.MapHealthChecks("/healthz");
	}

	public void Install(IApplicationBuilder webApplication)
	{
		throw new NotImplementedException();
	}
}
