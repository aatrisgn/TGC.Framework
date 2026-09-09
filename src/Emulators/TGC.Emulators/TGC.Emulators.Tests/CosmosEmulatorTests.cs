using System.Net;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

namespace TGC.Emulators.Tests;

public sealed class CosmosEmulatorTests : IAsyncLifetime
{
    private const string Endpoint = "https://localhost:8081/";
    private const string AccountKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
    private const string DatabaseId = "tgc-emulator-tests-db";
    private const string ContainerId = "tgc-emulator-tests-container";

    private CosmosClient _client = null!;

    public async Task InitializeAsync()
    {
        _client = new CosmosClient(Endpoint, AccountKey, new CosmosClientOptions
        {
            ConnectionMode = ConnectionMode.Gateway,
            ServerCertificateCustomValidationCallback = (_, _, _) => true,
        });

        await EmulatorRetry.UntilSuccessAsync(() => _client.ReadAccountAsync());
    }

    public async Task DisposeAsync()
    {
        try
        {
            await _client.GetDatabase(DatabaseId).DeleteAsync();
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
        }
        finally
        {
            _client.Dispose();
        }
    }

    [Fact]
    public async Task CreateDatabaseContainerAndItem_ThenReadItBack()
    {
        var database = (await _client.CreateDatabaseIfNotExistsAsync(DatabaseId)).Database;
        var container = (await database.CreateContainerIfNotExistsAsync(ContainerId, "/id")).Container;

        var item = new TestItem("item-1", "hello from the Cosmos emulator");
        await container.CreateItemAsync(item, new PartitionKey(item.Id));

        var response = await container.ReadItemAsync<TestItem>(item.Id, new PartitionKey(item.Id));

        Assert.Equal(item.Id, response.Resource.Id);
        Assert.Equal(item.Value, response.Resource.Value);
    }

    private sealed record TestItem([property: JsonProperty("id")] string Id, string Value);
}
