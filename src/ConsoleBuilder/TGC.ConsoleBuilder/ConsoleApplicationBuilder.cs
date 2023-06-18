using Microsoft.Extensions.DependencyInjection;
using TGC.ConsoleBuilder.Abstractions;
using TGC.ConsoleBuilder.Exceptions;

namespace TGC.ConsoleBuilder;

internal class ConsoleApplicationBuilder : IConsoleApplicationBuilder
{
	public IConsoleApplication Build()
	{
		var serviceCollection = new ServiceCollection();

		return BuildConsoleApplication(serviceCollection);
	}

	public IConsoleApplication Build(Action<IServiceCollection> action)
	{
		var serviceCollection = new ServiceCollection();

		action.Invoke(serviceCollection);

		return BuildConsoleApplication(serviceCollection);
	}

	private IConsoleApplication BuildConsoleApplication(IServiceCollection serviceCollection)
	{
		ResolveSystemServices(serviceCollection);

		var serviceProvider = serviceCollection.BuildServiceProvider();

		return new ConsoleApplication(serviceProvider);
	}

	private void ResolveSystemServices(IServiceCollection serviceCollection)
	{
		ResolveIConsoleInjectionBuilders(serviceCollection);
		ResolveStartService<ISyncStartService, NoSyncStartServiceFoundException>(serviceCollection);
		ResolveStartService<IAsyncStartService, NoAsyncStartServiceFoundException>(serviceCollection);
	}

	private void ResolveIConsoleInjectionBuilders(IServiceCollection serviceCollection)
	{
		var interfaceType = typeof(IConsoleInjectionBuilder);

		var injectors = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(s => s.GetTypes())
			.Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass);

		if (injectors.Any())
		{
			foreach (var injector in injectors)
			{
				var activatedInstance = Activator.CreateInstance(injector);
				if (activatedInstance != null)
				{
					var castedInstance = (IConsoleInjectionBuilder)activatedInstance;
					castedInstance.Configure(serviceCollection);
				}
			}
		}
	}

	private void ResolveStartService<TInterface, TException>(IServiceCollection serviceCollection) where TException : Exception
	{
		var interfaceType = typeof(TInterface);

		var injectors = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(s => s.GetTypes())
			.Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass);

		if (injectors.Any())
		{
			if (injectors.Count() > 1)
			{
				throw new DublicateStartServiceException($"More than one implementation of {nameof(TException)} was found injected. Inject only one.");
			}

			var implementation = injectors.First(i => i.GetConstructor(Type.EmptyTypes) != null);

			var instanceOfImplementation = Activator.CreateInstance(implementation);

			if (instanceOfImplementation != null)
			{
				serviceCollection.AddSingleton(typeof(TInterface), implementation);
			}
			else
			{
				var exceptionInstance = Activator.CreateInstance<TException>();

				throw exceptionInstance;
			}
		}
	}
}
