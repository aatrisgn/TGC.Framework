namespace TGC.AzureTableStorage.Configuration;

public interface IStorageConfigurationBuilder
{
	string? AccountConnectionString { get; set; }
	string? StorageAccountUrl { get; set; }
	bool StubServices { get; set; }
	bool UseManagedIdentity { get; set; }
}