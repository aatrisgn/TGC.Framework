using Azure.Data.Tables;

namespace TGC.AzureTableStorage;

internal class TableStorageContext : ITableStorageContext
{
	private readonly TableServiceClient _serviceClient;

	public TableStorageContext(TableServiceClient serviceClient)
	{
		_serviceClient = serviceClient;
	}

	public TableClient CreateClient(string tableName)
	{
		return _serviceClient.GetTableClient(tableName);
	}
}
