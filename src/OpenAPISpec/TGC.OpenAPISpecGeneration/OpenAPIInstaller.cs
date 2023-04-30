using Microsoft.AspNetCore.Builder;
using TGC.WebApiBuilder;

namespace TGC.OpenAPISpecGeneration;

internal class OpenAPIInstaller : IApplicationBuilderInstaller
{
	public void Install(IApplicationBuilder webApplication)
	{
		webApplication.UseSwagger();
		webApplication.UseSwaggerUI();
	}
}
