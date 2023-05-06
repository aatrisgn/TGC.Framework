using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace TGC.WebApiBuilder.Tests;

public class WebApplicationBuilderExtensionsTests
{
	[Test]
	public async Task WHEN_InjectingServicesViaReflection_THEN_RelevantServicesAreFound()
	{
		var args = new string[0];
		var webApplicationBuilder = WebApplication.CreateBuilder(args);

		var app = webApplicationBuilder.BuildWebAPI();

		var injectedService = app.Services.GetRequiredService<IApplicationBuilderInstaller>();

		injectedService.Should().NotBeNull();
	}

	[Test]
	public async Task WHEN_AppInstallersAreInjected_THEN_InstallersAreExecuted()
	{
		var args = new string[0];
		var webApplicationBuilder = WebApplication.CreateBuilder(args);

		var app = webApplicationBuilder.BuildWebAPI();

		TestInstaller.HasBeenActived.Should().BeTrue();
	}
}

[DependencyInjectionBuilder]
public class TestInjector : IDependencyInjecter
{
	public void InjectServices(IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<IApplicationBuilderInstaller, TestInstaller>();
	}
}

public class TestInstaller : IApplicationBuilderInstaller
{
	public static bool HasBeenActived = false;
	public void Install(IApplicationBuilder webApplication)
	{
		HasBeenActived = true;
	}
}