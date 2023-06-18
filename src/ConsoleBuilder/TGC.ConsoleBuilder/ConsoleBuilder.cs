using TGC.ConsoleBuilder.Abstractions;

namespace TGC.ConsoleBuilder;

public class ConsoleBuilder
{
	public static IConsoleApplicationBuilder CreateApp(string[] args)
	{
		return new ConsoleApplicationBuilder();
	}

	public static IConsoleApplicationBuilder CreateApp()
	{
		return new ConsoleApplicationBuilder();
	}
}
