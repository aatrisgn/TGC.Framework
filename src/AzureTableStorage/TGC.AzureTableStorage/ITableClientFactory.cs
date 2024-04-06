using Azure.Data.Tables;

namespace TGC.AzureTableStorage;
public interface ITableClientFactory
{
	TableClient GetClient(string tableName);
}
