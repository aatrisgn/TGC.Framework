using Azure.Data.Tables;

namespace TGC.AzureTableStorage;

public interface ITableStorageContext
{
	TableClient CreateClient(string tableName);
}