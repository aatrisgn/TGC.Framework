using Azure.Identity;
using Microsoft.Azure.Cosmos;
using TGC.Cosmos.Abstractions;
using TGC.Cosmos.Attributes;

namespace TGC.Cosmos;
internal class CosmosClientConnectionFactory : ICosmosClientConnectionFactory
{
	private readonly ICosmosConfiguration _cosmosConfiguration;

	private CosmosClient? _cosmosClient;
	private readonly Dictionary<string, Container> containerRegistry = new();

	private object _clientLock = new();
	private object _containerLock = new();

	public CosmosClientConnectionFactory(ICosmosConfiguration cosmosConfiguration)
	{
		_cosmosConfiguration = cosmosConfiguration;
	}

	public async Task<Container> GetContainerAsync<T>()
	{
		Type type = typeof(T);

		containerRegistry.TryGetValue(type.Name, out var container);

		if (container == null)
		{
			container = await InitializeContainerAsync(type);
		}

		return container;
	}

	public CosmosClient GetClient()
	{
		if (_cosmosClient == null)
		{
			lock (_clientLock)
			{
				if (_cosmosClient == null)
				{
					return InitializeClient();
				}
			}
		}

		return _cosmosClient;
	}

	private CosmosClient InitializeClient()
	{
		CosmosClient cosmosClient;

		if (_cosmosConfiguration.UseManagedIdentity)
		{
			var azureCredentials = new DefaultAzureCredential();

			if (_cosmosConfiguration.Endpoint == null)
			{
				throw new InvalidOperationException("Cosmos Configuration Endpoint is not set.");
			}

			cosmosClient = new CosmosClient(_cosmosConfiguration.Endpoint, azureCredentials);
		}
		else
		{
			var cosmosConnectionStringEnvironment = Environment.GetEnvironmentVariable(Constants.CONNECTIONSTRING_ENVIRONMENTVARIABLENAME);

			if (cosmosConnectionStringEnvironment == null)
			{
				cosmosClient = new CosmosClient(_cosmosConfiguration.ConnectionString);
			}
			else
			{
				cosmosClient = new CosmosClient(cosmosConnectionStringEnvironment);
			}
		}

		_cosmosClient = cosmosClient;
		return _cosmosClient;
	}

	private async Task<Container> InitializeContainerAsync(Type type)
	{
		var database = await InitializeDatabaseAsync(_cosmosConfiguration.DatabaseName);

		var containerName = GetContainerName(type);

		if (_cosmosConfiguration.CanManageInstance())
		{
			var containerProperties = new ContainerProperties(containerName, "/id");
			var container = await database.CreateContainerIfNotExistsAsync(containerProperties);

			containerRegistry.Add(type.Name, container);

			return container;
		}
		else
		{
			var container = database.GetContainer(containerName);

			containerRegistry.Add(type.Name, container);

			return container;
		}
	}

	private async Task<Database> InitializeDatabaseAsync(string databaseName)
	{
		if (_cosmosConfiguration.CanManageInstance())
		{
			var database = await GetClient().CreateDatabaseIfNotExistsAsync(databaseName);
			return database.Database;
		}

		return GetClient().GetDatabase(databaseName);
	}

	private string GetContainerName(Type type)
	{
		if (Attribute.IsDefined(type, typeof(IsolatedRepositoryAttribute)))
		{
			var containerAttribute = (IsolatedRepositoryAttribute?)Attribute.GetCustomAttribute(type, typeof(IsolatedRepositoryAttribute));
			if (containerAttribute != null)
			{
				return containerAttribute.CollectionName;
			}
		}
		else if (Attribute.IsDefined(type, typeof(IsolatedTenantRepositoryAttribute)))
		{
			var containerAttribute = (IsolatedTenantRepositoryAttribute?)Attribute.GetCustomAttribute(type, typeof(IsolatedTenantRepositoryAttribute));
			if (containerAttribute != null)
			{
				return containerAttribute.CollectionName;
			}
		}
		else if (Attribute.IsDefined(type, typeof(RepositoryAttribute)) || Attribute.IsDefined(type, typeof(TenantRepositoryAttribute)))
		{
			return _cosmosConfiguration.PrimaryContainerName;
		}

		throw new InvalidOperationException("Type has not been decorated with valid attribute for Container mapping.");
	}
}
