namespace TGC.Cosmos.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class IsolatedTenantRepositoryItem : Attribute
{
	public string CollectionName { get; }
	public int SchemaVersion { get; }

	public IsolatedTenantRepositoryItem(string collectionName)
	{
		CollectionName = collectionName;
	}

	public IsolatedTenantRepositoryItem(string collectionName, int schemaVersion)
	{
		CollectionName = collectionName;
		SchemaVersion = schemaVersion;
	}
}
