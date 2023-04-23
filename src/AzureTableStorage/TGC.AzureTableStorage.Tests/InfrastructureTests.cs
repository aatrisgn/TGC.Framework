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
		serviceCollection.AddStorageRepository<TestTableItem>();

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
