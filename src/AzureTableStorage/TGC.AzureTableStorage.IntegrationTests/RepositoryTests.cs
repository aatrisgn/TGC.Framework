using Azure.Data.Tables;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TGC.AzureTableStorage.Extensions;
using TGC.AzureTableStorage.IoC;

namespace TGC.AzureTableStorage.IntegrationTests;

public class RepositoryTests
{
    private readonly IServiceProvider _serviceProvider;
    public RepositoryTests()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddAzureTableStorage(configuration =>
        {
            configuration.AccountConnectionString = "";
            configuration.StubServices = false;
            configuration.UseManagedIdentity = false;
        });
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
    
    [Fact]
    public async Task GIVEN_ConfiguredIntegrationViaIoC_THEN_ItIsPossibleToCreateItems()
    {
        var repository = _serviceProvider.GetService<IAzureTableStorageRepository<TestEntity>>();

        if (repository is null)
        {
            throw new NullReferenceException("repository is null and shouldn't be null.");
        }
        
        await repository.CreateAsync(new TestEntity
        {
            Name = "Test value"
        });
        
        var locatedItem = repository.QueryAsync(e => e.Name == "Test value");
        var singleItem = await locatedItem.SingleAsync();
        
        singleItem.Name.Should().Be("Test value");
    }

    [TableItem("TestEntities")]
    private class TestEntity : AzureTableItem, ITableEntity
    {
        public string Name { get; set; }
    }
}