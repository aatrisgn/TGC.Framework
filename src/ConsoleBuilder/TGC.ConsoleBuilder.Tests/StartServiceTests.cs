using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TGC.ConsoleBuilder.Abstractions;

namespace TGC.ConsoleBuilder.Tests;

internal class StartServiceTests
{
	[Test]
	public async Task GIVEN_RegisteredIAsyncStartServiceImplementation_THEN_ExecutionIsSuccessful()
	{
		var SUT = ConsoleBuilder.CreateApp().Build();
		await SUT.RunAsync();

		AsyncStartService.HasExecuted.Should().BeTrue();
	}

	[Test]
	public async Task GIVEN_RegisteredISyncStartServiceImplementation_THEN_ExecutionIsSuccessful()
	{
		var SUT = ConsoleBuilder.CreateApp().Build();
		SUT.Run();

		SyncStartService.HasExecuted.Should().BeTrue();
	}

	[Test]
	public async Task GIVEN_NoRegisteredStartServiceImplementations_WHEN_RunningFromServiceProviderWithRegisteredServices_THEN_MethodIsExecuted()
	{
		var mockedStartup = new Mock<ICustomStartUpService>();

		var SUT = ConsoleBuilder.CreateApp()
			.Build(serviceCollection => serviceCollection.AddSingleton(mockedStartup));

		SUT.RunFromServiceProvider(serviceProvider => serviceProvider.GetRequiredService<Mock<ICustomStartUpService>>().Object.Run());

		mockedStartup.Verify(x => x.Run());
	}

	[Test]
	public async Task GIVEN_NoRegisteredStartServiceImplementations_WHEN_RunningFromServiceProviderWithNoRegisteredServices_THEN_ExceptionIsThrown()
	{
		var SUT = ConsoleBuilder.CreateApp().Build();

		Action act = () => SUT.RunFromServiceProvider(serviceProvider => serviceProvider.GetRequiredService<ICustomStartUpService>().Run());

		act.Should().Throw<InvalidOperationException>().WithMessage("No service for type 'TGC.ConsoleBuilder.Tests.ICustomStartUpService' has been registered.");
	}
}

public class AsyncStartService : IAsyncStartService
{
	public static bool HasExecuted;
	public Task RunAsync()
	{
		HasExecuted = true;
		return Task.CompletedTask;
	}
}

public class SyncStartService : ISyncStartService
{
	public static bool HasExecuted;
	public void Run()
	{
		HasExecuted = true;
	}
}

public interface ICustomStartUpService
{
	void Run();
}
