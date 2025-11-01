namespace TGC.AzureTableStorage.Configuration;

/// <summary>
/// Defines configuration settings for accessing Azure Table Storage.
/// </summary>
public interface IStorageConfiguration
{
	/// <summary>
	/// Gets or sets the connection string used to access the Azure Storage account.
	/// </summary>
	string? AccountConnectionString { get; set; }

	/// <summary>
	/// Gets or sets the URL of the Azure Storage account.
	/// </summary>
	string? StorageAccountUrl { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether to use managed identity for authentication.
	/// </summary>
	bool UseManagedIdentity { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether to use managed identity for authentication.
	/// </summary>
	Guid ManagedIdentityId { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether to stub services for testing or development purposes.
	/// </summary>
	bool StubServices { get; set; }
}
