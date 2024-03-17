namespace TGC.AzureTableStorage.Configuration;

public class StorageConfiguration : IStorageConfiguration
{
	public string? AccountConnectionString { get; set; }
	public bool UseManagedIdentity { get; set; }
	public string? StorageAccountUrl { get; set; }
	public bool StubServices { get; set; }
}
