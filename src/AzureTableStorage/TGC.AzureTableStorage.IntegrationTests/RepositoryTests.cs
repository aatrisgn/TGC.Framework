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
            configuration.AccountConnectionString = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
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
    public async Task GIVEN_ExistingItem_THEN_ExistsByIdShouldReturnTrue()
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
        
        var newItem = await repository.GetSingleAsync(i => i.Name == "Test value");
        
        var locatedItem = await repository.ExistsByIdAsync(Guid.Parse(newItem.RowKey));
        locatedItem.Should().BeTrue();
    }
    
    [Fact]
    public async Task GIVEN_ExistingItem_THEN_ExistsShouldReturnTrue()
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
        
        var locatedItem = await repository.ExistsAsync(i => i.Name == "Test value");
        locatedItem.Should().BeTrue();
    }
    
    [Fact]
    public async Task GIVEN_ExistingItem_WHEN_DeletingEntity_THEN_ExistsShouldReturnFalse()
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
        
        var newItem = await repository.GetSingleAsync(i => i.Name == "Test value");
        
        await repository.DeleteByIdAsync(Guid.Parse(newItem.RowKey));
        
        var locatedItem = await repository.ExistsByIdAsync(Guid.Parse(newItem.RowKey));
        locatedItem.Should().BeFalse();
    }
    
    [Fact]
    public async Task GIVEN_NonExistingItem_THEN_ExistsByIdShouldReturnTrue()
    {
        var repository = _serviceProvider.GetService<IAzureTableStorageRepository<TestEntity>>();

        if (repository is null)
        {
            throw new NullReferenceException("repository is null and shouldn't be null.");
        }
        
        var locatedItem = await repository.ExistsByIdAsync(Guid.NewGuid());
        locatedItem.Should().BeFalse();
    }
    
    [Fact]
    public async Task GIVEN_NonExistingItem_THEN_ExistsShouldReturnTrue()
    {
        var repository = _serviceProvider.GetService<IAzureTableStorageRepository<TestEntity>>();

        if (repository is null)
        {
            throw new NullReferenceException("repository is null and shouldn't be null.");
        }
        
        var locatedItem = await repository.ExistsAsync(i => i.Name == "aqweqwdqw");
        locatedItem.Should().BeFalse();
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

    [Fact]
    public async Task GIVEN_SingeItemInRepository_THEN_ItIsReturnedWithCorrectCapitalization()
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
    public async Task GIVEN_ExistingItemInStorage_WHEN_TryingToDelete_THEN_EntityIsRemoved()
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
        
        var idOfDeletedEntity = await repository.DeleteAsync(locatedItem);
        idOfDeletedEntity.Should().Be( Guid.Parse(locatedItem.RowKey));
        
        var matchingItems = await repository.GetAllAsync(e => e.RowKey == idOfDeletedEntity.ToString());
        matchingItems.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GIVEN_ExistingItemInStorage_THEN_EnsureCorrectValuesAreReturned()
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