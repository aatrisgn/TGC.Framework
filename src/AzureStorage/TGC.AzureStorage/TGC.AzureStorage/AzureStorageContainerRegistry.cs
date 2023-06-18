using Azure.Identity;
using Azure.Storage.Blobs;

namespace TGC.AzureStorage;
internal class AzureStorageContainerRegistry : IAzureStorageContainerRegistry
{
	private BlobServiceClient blobServiceClient;
	private Dictionary<string, BlobContainerClient> containerRegistry = new Dictionary<string, BlobContainerClient>();

	public AzureStorageContainerRegistry(string connectionString)
	{
		blobServiceClient = new BlobServiceClient(connectionString);
	}

	public AzureStorageContainerRegistry(string storageUri, DefaultAzureCredential defaultAzureCredential)
	{
		blobServiceClient = new BlobServiceClient(new Uri(storageUri), defaultAzureCredential);
	}

	public IAzureStorageContainer? GetContainerContext(string containerName1)
	{
		blobServiceClient.

		throw new NotImplementedException();
	}
}
