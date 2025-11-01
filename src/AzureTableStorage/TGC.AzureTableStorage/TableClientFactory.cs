using Azure.Core;
using Azure.Data.Tables;
using Azure.Identity;
using TGC.AzureTableStorage.Configuration;

namespace TGC.AzureTableStorage;
internal sealed class TableClientFactory(IStorageConfiguration storageConfiguration) : ITableClientFactory
{
	private readonly IStorageConfiguration _storageConfiguration = storageConfiguration;

	private TableServiceClient? _serviceClient;
	private readonly Dictionary<string, TableClient> _tableServiceClients = new();

	//Should be updated to new lock type in .Net 9
	private readonly object _lock = new();

	public TableClient GetClient(string tableName)
	{
		var hasExistingTableClient = _tableServiceClients.TryGetValue(tableName, out var tableClient);
		if (hasExistingTableClient && tableClient is not null)
		{
			return tableClient;
		}

		lock (_lock)
		{
			_serviceClient ??= this.InstantiateServiceClient();

			// It may seem redundant to check again, but if multiple threads has gone in line for the lock, the later threads will not know that it has been created and will therefore retry to create it creating unnecessary client connections.
			var locatedTableClient = _tableServiceClients.TryGetValue(tableName, out var innerTableClient);
			if (locatedTableClient && innerTableClient is not null)
			{
				return innerTableClient;
			}

			var newTableClient = _serviceClient.GetTableClient(tableName);
			_tableServiceClients.Add(tableName, newTableClient);

			newTableClient.CreateIfNotExists();

			return newTableClient;
		}
	}

	private TableServiceClient InstantiateServiceClient()
	{
		if (_storageConfiguration.UseManagedIdentity)
		{
			if (!string.IsNullOrEmpty(_storageConfiguration.StorageAccountUrl))
			{
				TokenCredential credential;
				if (_storageConfiguration.ManagedIdentityId != Guid.Empty)
				{
					credential = new ManagedIdentityCredential(
						ManagedIdentityId.FromUserAssignedClientId(_storageConfiguration.ManagedIdentityId.ToString()));
				}
				else
				{
					credential = new DefaultAzureCredential();
				}
				return new TableServiceClient(new Uri(_storageConfiguration.StorageAccountUrl), credential);
			}

			throw new ArgumentNullException($"{_storageConfiguration.StorageAccountUrl} is not defined.");
		}

		return new TableServiceClient(_storageConfiguration.AccountConnectionString);
	}
}
