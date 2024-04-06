using Azure.Data.Tables;
using Azure.Identity;
using TGC.AzureTableStorage.Configuration;

namespace TGC.AzureTableStorage;
internal sealed class TableClientFactory(IStorageConfiguration storageConfiguration) : ITableClientFactory
{
	private readonly IStorageConfiguration _storageConfiguration = storageConfiguration;

	private TableServiceClient? _serviceClient;
	private readonly Dictionary<string, TableClient> _tableServiceClients = new Dictionary<string, TableClient>();

	private readonly object _lock = new();

	public TableClient GetClient(string tableName)
	{
		_tableServiceClients.TryGetValue(tableName, out var tableClient);
		if (tableClient != null)
		{
			return tableClient;
		}
		else
		{
			lock (_lock)
			{
				_serviceClient ??= this.InstantiateServiceClient();

				// It may seem redundant to check again, but if multiple threads has gone in line for the lock, the later threads will not know that it has been created and will therefore retry to create it creating unnecessary client connections.
				_tableServiceClients.TryGetValue(tableName, out var innerTableClient);
				if (innerTableClient != null)
				{
					return innerTableClient;
				}

				var newTableClient = _serviceClient.GetTableClient(tableName);
				_tableServiceClients.Add(tableName, newTableClient);

				newTableClient.CreateIfNotExists();

				return newTableClient;
			}
		}
	}

	private TableServiceClient InstantiateServiceClient()
	{
		if (_storageConfiguration.UseManagedIdentity)
		{
			if (!string.IsNullOrEmpty(_storageConfiguration.StorageAccountUrl))
			{
				return new TableServiceClient(new Uri(_storageConfiguration.StorageAccountUrl), new DefaultAzureCredential());
			}
			else
			{
				throw new ArgumentNullException($"{_storageConfiguration.StorageAccountUrl} is not defined.");
			}
		}
		else
		{
			return new TableServiceClient(_storageConfiguration.AccountConnectionString);
		}
	}
}
