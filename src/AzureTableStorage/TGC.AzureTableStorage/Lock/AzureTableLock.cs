using Azure.Data.Tables;

namespace TGC.AzureTableStorage;

public class AzureTableLock<T> : IAzureTableReadLock<T>, IAzureTableWriteLock<T> where T : class, ITableEntity, new()
{
	private readonly object _lock;

	private bool useLock;

	public object Lock => _lock;

	public AzureTableLock()
	{
		_lock = new object();
		useLock = false;
	}

	public void RemoveLock()
	{
		useLock = false;
	}

	public bool UseLock()
	{
		return useLock;
	}

	public void SetLock()
	{
		useLock = true;
	}
}
