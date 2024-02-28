namespace TGC.Cosmos;
public class CosmosConfiguration : ICosmosConfiguration
{
	public string? ConnectionString { get; set; }
	public string? Endpoint { get; set; }
	public bool UseManagedIdentity { get; set; }
	public bool Enabled { get; set; }
	public Guid ManagedIdentityId { get; set; }
	public required string DatabaseName { get; set; }
}
