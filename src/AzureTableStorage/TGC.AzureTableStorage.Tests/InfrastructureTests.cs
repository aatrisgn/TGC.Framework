using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TGC.AzureTableStorage.Configuration;
using TGC.AzureTableStorage.IoC;

namespace TGC.AzureTableStorage.Tests;

public class InfrastructureTests
{
	private IServiceProvider _serviceProvider;

	[SetUp]
	public void Setup()
	{
		var serviceCollection = CreateServiceCollection();
		serviceCollection.AddAzureTableStorage();

		_serviceProvider = serviceCollection.BuildServiceProvider();
	}

	[Test]
	public void GIVEN_ConfiguredIoC_THEN_ContextIsAvailable()
	{
		var context = _serviceProvider.GetService<ITableStorageContext>();
		context.Should().NotBeNull();
	}

	[Test]
	public void GIVEN_ConfiguredIoC_THEN_StorageOptionsIsAvailable()
	{
		var configurationInterface = _serviceProvider.GetService<IStorageConfiguration>();
		configurationInterface.Should().NotBeNull();
	}

	[Test]
	[Ignore("Figure out how to fix")]
	public void GIVEN_ConfiguredIoC_WHEN_ConnectionStringIsDirectlyGiven_THEN_StorageOptionsIsAvailable()
	{
		var testConnectionStringValue = "TestValue";
		var serviceCollection = CreateServiceCollection();
		serviceCollection.AddAzureTableStorage(testConnectionStringValue);

		var localProvider = serviceCollection.BuildServiceProvider();

		var configurationInterface = localProvider.GetRequiredService<IStorageConfiguration>();

		configurationInterface.Should().NotBeNull();
		configurationInterface.AccountConnectionString.Should().Be(testConnectionStringValue);
	}

	private IServiceCollection CreateServiceCollection()
	{
		return new ServiceCollection();
	}

	[TableItem("TestTable")]
	private class TestTableItem : AzureTableItem
	{
		public string? TestValueString { get; set; }
	}
}
