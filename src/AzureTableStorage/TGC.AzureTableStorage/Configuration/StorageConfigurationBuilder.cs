namespace TGC.AzureTableStorage.Configuration;
public class StorageConfigurationBuilder : IStorageConfigurationBuilder
{
	public string? AccountConnectionString { get; set; }
	public string? StorageAccountUrl { get; set; }
	public bool UseManagedIdentity { get; set; }
	public bool StubServices { get; set; }

	internal StorageConfiguration ToConfiguration()
	{
		return new StorageConfiguration(this);
	}
}
