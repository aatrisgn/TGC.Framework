using Microsoft.Extensions.DependencyInjection;
using TGC.AzureTableStorage.IoC;

namespace TGC.AzureTableStorage.TestTesting;

public class StubbedRepositoryTests
{
	private IServiceProvider _serviceProvider;
	[SetUp]
	public void Setup()
	{
		var serviceCollection = new ServiceCollection()
			.AddAzureTableStorage(options => options.StubServices = true);

		_serviceProvider = serviceCollection.BuildServiceProvider();
	}

	[Test]
	public async Task GIVEN_ExistingItems_WHEN_FetchingAllItems_THEN_ReturnAllItems()
	{
		var repository = _serviceProvider.GetRequiredService<IAzureTableStorageRepository<TestEntity>>();

		await repository.CreateAsync(new TestEntity
		{
			Name = "John Doe",
			Age = 99
		});

		await repository.CreateAsync(new TestEntity
		{
			Name = "Jane Smith",
			Age = 11
		});

		var results = await repository.GetAllAsync(t => t != null);

		Assert.That(results.Count(), Is.EqualTo(2));
	}

	[Test]
	public async Task GIVEN_NoExistingItems_WHEN_CreatingNewItem_THEN_ItCanBeFound()
	{
		var entityName = "John Doe";
		var repository = _serviceProvider.GetRequiredService<IAzureTableStorageRepository<TestEntity>>();

		await repository.CreateAsync(new TestEntity
		{
			Name = entityName,
			Age = 99
		});

		var itemExist = await repository.ExistsAsync(t => t.Name == entityName);
	}

	[TableItem("TestEntity")]
	public class TestEntity : AzureTableItem
	{
		public string? Name { get; set; }
		public int Age { get; set; }
	}
}
