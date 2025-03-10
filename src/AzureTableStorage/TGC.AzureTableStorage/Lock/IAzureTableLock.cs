using Azure.Data.Tables;

namespace TGC.AzureTableStorage;

public interface IAzureTableLock<T> where T : class, ITableEntity, new()
{
	object Lock { get; }
	void RemoveLock();
	bool UseLock();
	void SetLock();
}
