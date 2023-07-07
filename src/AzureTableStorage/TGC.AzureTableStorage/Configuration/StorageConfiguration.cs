namespace TGC.AzureTableStorage.Configuration;

public class StorageConfiguration : IStorageConfiguration
{
	public string? AccountConnectionString { get; init; }
	public bool UseManagedIdentity { get; init; }
	public string? StorageAccountUrl { get; init; }
}
