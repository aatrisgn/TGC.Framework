namespace TGC.AzureTableStorage.Configuration;

internal class StorageConfiguration : IStorageConfiguration
{
	public string? AccountConnectionString { get; init; }
	public bool UseManagedIdentity { get; init; }
	public string? StorageAccountUrl { get; init; }
	public bool StubServices { get; init; }

	public StorageConfiguration()
	{
	}

	public StorageConfiguration(IStorageConfigurationBuilder storageConfigurationBuilder)
	{
		this.AccountConnectionString = storageConfigurationBuilder.AccountConnectionString;
		this.UseManagedIdentity = storageConfigurationBuilder.UseManagedIdentity;
		this.StorageAccountUrl = storageConfigurationBuilder.StorageAccountUrl;
		this.StubServices = storageConfigurationBuilder.StubServices;
	}
}
