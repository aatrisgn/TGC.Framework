using Azure.Data.Tables;
using Microsoft.Extensions.Options;
using TGC.AzureTableStorage.Configuration;

namespace TGC.AzureTableStorage;

internal class TableStorageContext : ITableStorageContext
{
	private readonly TableServiceClient serviceClient;
	private readonly IStorageConfiguration _storageConfiguration;

	public TableStorageContext(IOptions<StorageConfiguration> storageConfiguration)
	{
		_storageConfiguration = storageConfiguration.Value;
		serviceClient = new TableServiceClient(_storageConfiguration.AccountConnectionString);
	}

	public TableClient CreateClient(string tableName)
	{
		return serviceClient.GetTableClient(tableName);
	}
}
