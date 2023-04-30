using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace TGC.WebApiBuilder;

public static class WebApplicationBuilderExtensions
{
	public static WebApplication BuildWebAPI(this WebApplicationBuilder builder)
	{
		var serviceProvider = builder.Services.BuildServiceProvider();
		var app = builder.Build();

		var applicationInstallers = serviceProvider.GetServices<IApplicationBuilderInstaller>();

		foreach (var applicationInstaller in applicationInstallers)
		{
			applicationInstaller.Install(app);
		}

		return app;
	}
}
