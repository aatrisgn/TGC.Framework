namespace TGC.AzureStorage;
public interface IAzureStorageContainer
{
	void DownloadAsync(string v, string v1);
	object DownloadAsync(string v);
	object GetAsync(string v);
	object ListAsync(Func<object, bool> value);
	object ListAsync();
	void UploadAsync(string v, AccessTier cold);
}
