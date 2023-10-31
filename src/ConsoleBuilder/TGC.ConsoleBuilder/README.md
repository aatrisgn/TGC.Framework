# TGC.ConsoleBuilder

## Using the ConsoleBuilder

There are two ways of using the ConsoleBuilder:

1. You run it from a manually defined injected service
2. You run it via Run()/RunAsync();

### Running via injected service

	var consoleAppBuilder = ConsoleBuilder.CreateApp(args);
	var consoleApp = consoleAppBuilder.Build();

	consoleApp.RunFromServiceProvider(s => s.GetRequiredService<ITestService>().DoStuff());

If you run it via an injected service, this service needs to be injected. This is done via implementing the interface `IConsoleInjectionBuilder`. It is in this implementation you specify your dependencies. It could look like the following:

	internal class ConsoleInjectionBuilder : IConsoleInjectionBuilder
	{
		public void Configure(IServiceCollection serviceCollection)
		{
			serviceCollection.AddScoped<ITestService, TestService>();
		}
	}

### Running via Run()/RunAsync()

	var consoleAppBuilder = ConsoleBuilder.CreateApp(args);
	var consoleApp = consoleAppBuilder.Build();

	await consoleApp.RunAsync();
	// OR
	await consoleApp.Run();

To be able to use Run or RunAsync you need to have implemented the interface IAsyncStartService or ISyncStartService. They are both executed via dependency injection so you are able to access your dependencies registered in IConsoleInjectionBuilder directly from your implementation.

Example of setup:

`IConsoleInjectionBuilder`

	internal class ConsoleInjectionBuilder : IConsoleInjectionBuilder
	{
		public void Configure(IServiceCollection serviceCollection)
		{
			serviceCollection.AddScoped<ITestService, TestService>();
		}
	}

`IAsyncStartService`

	internal class AsyncRunner : IAsyncStartService
	{
		private readonly ITestService _testService;

		public AsyncRunner(ITestService testService)
		{
			_testService = testService;
		}

		public Task RunAsync()
		{
			_testService.ResolveImportantStuff();
			_testService.StartBatchHandling();
			//etc...
		}
	}

## Roadmap

### Implement Host Builder
At the moment, things has been manually wired. This is not ideal. Instead, it should wrap IHost.

## Changelog

### 1.1.3
Added source code reference.

### 1.1.2
Added project url and updated README.md

### 0.1.0
Initial version - Not tested and ready for usage.