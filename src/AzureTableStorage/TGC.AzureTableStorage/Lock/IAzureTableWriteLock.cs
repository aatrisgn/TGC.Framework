using Azure.Data.Tables;

namespace TGC.AzureTableStorage;

public interface IAzureTableWriteLock<T> : IAzureTableLock<T> where T : class, ITableEntity, new()
{
}
