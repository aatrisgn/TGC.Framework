using Microsoft.Extensions.DependencyInjection;
using TGC.ConsoleBuilder.Abstractions;
using TGC.ConsoleBuilder.Exceptions;

namespace TGC.ConsoleBuilder;
internal class ConsoleApplication : IConsoleApplication
{
	private readonly IServiceProvider _serviceProvider;
	public ConsoleApplication(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public async Task RunAsync()
	{
		var asyncStartService = _serviceProvider.GetRequiredService<IAsyncStartService>();

		if (asyncStartService == null)
		{
			throw new NoAsyncStartServiceFoundException("No implementation of IAsyncStartService found. Remember implement interface to be able to RunAsync();");
		}

		await asyncStartService.RunAsync();
	}

	public void RunFromServiceProvider(Action<IServiceProvider> action)
	{
		action.Invoke(_serviceProvider);
	}

	public void Run()
	{
		var asyncStartService = _serviceProvider.GetRequiredService<ISyncStartService>();

		if (asyncStartService == null)
		{
			throw new NoSyncStartServiceFoundException("No implementation of IStartService found. Remember implement interface to be able to Run();");
		}

		asyncStartService.Run();
	}
}
