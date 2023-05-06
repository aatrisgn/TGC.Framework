using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace TGC.WebApiBuilder;

public static class WebApplicationBuilderExtensions
{
	public static WebApplication BuildWebAPI(this WebApplicationBuilder builder)
	{
		AddInjectors(builder.Services);

		builder.Services.AddControllers();
		var serviceProvider = builder.Services.BuildServiceProvider();
		var app = builder.Build();

		AddInstallers<IApplicationBuilderInstaller>(serviceProvider, (installer) => installer.Install(app));
		AddInstallers<IEndpointRouteBuilderInstaller>(serviceProvider, (installer) => installer.Install(app));

		app.UseHttpsRedirection();
		app.MapControllers();

		return app;
	}

	private static void AddInjectors(IServiceCollection serviceCollection)
	{
		var interfaceType = typeof(IDependencyInjecter);

		var injectors = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(s => s.GetTypes())
			.Where(p => interfaceType.IsAssignableFrom(p) && (p.GetCustomAttribute<DependencyInjectionBuilderAttribute>() != null));

		if (injectors.Any())
		{
			foreach (var injector in injectors)
			{
				var activatedInstance = Activator.CreateInstance(injector);
				if (activatedInstance != null)
				{
					var castedInstance = (IDependencyInjecter)activatedInstance;
					castedInstance.InjectServices(serviceCollection);
				}
			}
		}
	}

	private static void AddInstallers<T>(ServiceProvider serviceProvider, Action<T> value)
	{
		var applicationInstallers = serviceProvider.GetServices<T>();

		foreach (var applicationInstaller in applicationInstallers)
		{
			value.Invoke(applicationInstaller);
		}
	}
}
