namespace TGC.AzureTableStorage.Configuration;

public interface IStorageConfiguration
{
	string? AccountConnectionString { get; init; }
	string? StorageAccountUrl { get; init; }
	bool UseManagedIdentity { get; init; }
	bool StubServices { get; init; }
}
