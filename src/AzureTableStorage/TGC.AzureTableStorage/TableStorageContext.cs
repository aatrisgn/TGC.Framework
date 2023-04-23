using Azure.Data.Tables;
using TGC.AzureTableStorage.Configuration;

namespace TGC.AzureTableStorage;

internal class TableStorageContext : ITableStorageContext
{
	private readonly TableServiceClient serviceClient;
	private readonly IStorageConfiguration _storageConfiguration;

	public TableStorageContext(IStorageConfiguration storageConfiguration)
	{
		_storageConfiguration = storageConfiguration;
		serviceClient = new TableServiceClient(_storageConfiguration.AccountConnectionString);
	}

	public TableClient CreateClient(string tableName)
	{
		return serviceClient.GetTableClient(tableName);
	}
}
