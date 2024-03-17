namespace TGC.AzureTableStorage.Configuration;

public interface IStorageConfiguration
{
	string? AccountConnectionString { get; set; }
	string? StorageAccountUrl { get; set; }
	bool UseManagedIdentity { get; set; }
	bool StubServices { get; set; }
}
