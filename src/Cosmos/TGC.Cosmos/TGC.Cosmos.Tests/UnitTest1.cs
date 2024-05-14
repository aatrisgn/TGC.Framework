using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TGC.Cosmos.Abstractions;
using TGC.Cosmos.Attributes;
using TGC.Cosmos.Extensions;
using TGC.Cosmos.Models;

namespace TGC.Cosmos.Tests;

public class RepositoryTests
{
    private ServiceProvider _serviceProvider;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();

		services.ConfigureCosmos(configuration =>
        {
			configuration.Enabled = true;
			configuration.DatabaseName = "Test";
		});

        _serviceProvider = services.BuildServiceProvider();

	}

    [TearDown]
    public void TearDown()
    {
		_serviceProvider.Dispose();
	}

    [Test]
    public async Task Test1()
    {
        var soas = _serviceProvider.GetRequiredService<ICosmosRepository<PrivateEntity>>();

		await soas.CreateAsync(new PrivateEntity { SomeName = "SomeName", SomeNumber = 1, SomeBoolean = true });
		await soas.CreateAsync(new PrivateEntity { SomeName = "SomeName", SomeNumber = 1, SomeBoolean = true });
		await soas.CreateAsync(new PrivateEntity { SomeName = "SomeName", SomeNumber = 1, SomeBoolean = true });

        var result = await soas.GetAllAsync();

        result.Should().NotBeNull();

	}

    [Repository(1)]
    private class PrivateEntity : RepositoryEntity
    {
        public string SomeName { get; set; }
        public int SomeNumber { get; set; }
        public bool SomeBoolean { get; set; }
    }
}