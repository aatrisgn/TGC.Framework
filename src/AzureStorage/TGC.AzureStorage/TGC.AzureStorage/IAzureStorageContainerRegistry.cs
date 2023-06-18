namespace TGC.AzureStorage;
public interface IAzureStorageContainerRegistry
{
	IAzureStorageContainer? GetContainerContext(string containerName1);
}
