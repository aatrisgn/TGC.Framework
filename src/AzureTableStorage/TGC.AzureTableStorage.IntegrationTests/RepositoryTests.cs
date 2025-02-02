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
    
    [Fact]
    public async Task GIVEN_ExistingItemInStorage_THEN_ItIsPossibleToRetrieveAnEntityWithAllProperties()
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
        
        var locatedItem = await repository.GetSingleAsync(e => e.Name == "Test value");
        locatedItem.Name.Should().Be("Test value");
    }
    
    [Fact]
    public async Task GIVEN_ExistingItemInStorage_THEN_ItIsPossibleToRetrieveAllEntitiesWithAllProperties()
    {
        var repository = _serviceProvider.GetService<IAzureTableStorageRepository<TestEntity>>();

        if (repository is null)
        {
            throw new NullReferenceException("repository is null and shouldn't be null.");
        }
        
        await repository.CreateAsync(new TestEntity
        {
            Name = "Test value1",
            Score = 1.2
        });
        
        await repository.CreateAsync(new TestEntity
        {
            Name = "Test value2",
            Score = 12.3
        });
        
        await repository.CreateAsync(new TestEntity
        {
            Name = "Test value3",
            Score = 123.4
        });
        
        var locatedItem = await repository.GetAllAsync(e => true);
        
        locatedItem.Any(e => e.Name == "Test value1").Should().BeTrue();
        locatedItem.Any(e => e.Name == "Test value2").Should().BeTrue();
        locatedItem.Any(e => e.Name == "Test value3").Should().BeTrue();
        
        locatedItem.Any(e => e.Score == 1.2).Should().BeTrue();
        locatedItem.Any(e => e.Score == 12.3).Should().BeTrue();
        locatedItem.Any(e => e.Score == 123.4).Should().BeTrue();
    }

    [TableItem("TestEntities")]
    private class TestEntity : AzureTableItem
    {
        public string Name { get; set; }
        public double Score { get; set; }
    }
}