using Microsoft.Extensions.DependencyInjection;
using TGC.ConsoleBuilder.Abstractions;

namespace TGC.ConsoleBuilder.TestConsole;

internal class ConsoleInjectionBuilder : IConsoleInjectionBuilder
{
	public void Configure(IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<ITestService, TestService>();
	}
}
