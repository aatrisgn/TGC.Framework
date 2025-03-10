using Azure.Data.Tables;

namespace TGC.AzureTableStorage;

public interface IAzureTableReadLock<T> : IAzureTableLock<T> where T : class, ITableEntity, new()
{
}
