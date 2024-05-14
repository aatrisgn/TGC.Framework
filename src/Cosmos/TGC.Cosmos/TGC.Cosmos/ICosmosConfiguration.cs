
namespace TGC.Cosmos;

public interface ICosmosConfiguration
{
	string? ConnectionString { get; set; }
	string PrimaryContainerName { get; set; }
	bool Enabled { get; set; }
	string? Endpoint { get; set; }
	Guid ManagedIdentityId { get; set; }
	bool UseManagedIdentity { get; set; }
	string DatabaseName { get; set; }
	bool CanManageInstance();
}
