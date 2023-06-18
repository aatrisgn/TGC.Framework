using Microsoft.Extensions.DependencyInjection;

namespace TGC.ConsoleBuilder.Abstractions;
public interface IConsoleApplicationBuilder
{
	IConsoleApplication Build();
	IConsoleApplication Build(Action<IServiceCollection> action);
}
