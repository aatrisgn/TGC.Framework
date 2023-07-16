using Azure.Data.Tables;

namespace TGC.AzureTableStorage.Tests;
internal class StubbedTableStorageContext : ITableStorageContext
{
	public TableClient CreateClient(string tableName)
	{
		throw new NotImplementedException();
	}
}
