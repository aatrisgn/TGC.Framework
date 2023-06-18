namespace TGC.ConsoleBuilder.Abstractions;
public interface IConsoleApplication
{
	void RunFromServiceProvider(Action<IServiceProvider> action);
	Task RunAsync();
	void Run();
}
