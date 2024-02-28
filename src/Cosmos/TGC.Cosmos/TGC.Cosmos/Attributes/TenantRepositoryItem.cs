namespace TGC.Cosmos.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class TenantRepositoryItem : Attribute
{
	public string CollectionName { get; }
	public int SchemaVersion { get; }

	public TenantRepositoryItem(string collectionName)
	{
		CollectionName = collectionName;
	}

	public TenantRepositoryItem(string collectionName, int schemaVersion)
	{
		CollectionName = collectionName;
		SchemaVersion = schemaVersion;
	}
}
