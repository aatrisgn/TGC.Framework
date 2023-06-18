using Microsoft.Extensions.DependencyInjection;

namespace TGC.ConsoleBuilder.Abstractions;
public interface IConsoleInjectionBuilder
{
	void Configure(IServiceCollection serviceCollection);
}
