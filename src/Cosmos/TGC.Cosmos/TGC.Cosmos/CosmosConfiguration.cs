namespace TGC.Cosmos;
public class CosmosConfiguration : ICosmosConfiguration
{
	public string? ConnectionString { get; set; }
	public string PrimaryContainerName { get; set; } = "Items"; // Default value "Items
	public string? Endpoint { get; set; }
	public bool UseManagedIdentity { get; set; }
	public bool Enabled { get; set; }
	public Guid ManagedIdentityId { get; set; }
	public required string DatabaseName { get; set; }

	public bool CanManageInstance()
	{
		return !string.IsNullOrWhiteSpace(ConnectionString);
	}
}
