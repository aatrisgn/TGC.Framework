namespace TGC.Cosmos.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class TenantRepositoryAttribute : Attribute
{
	public string CollectionName { get; }
	public int SchemaVersion { get; }

	public TenantRepositoryAttribute(string collectionName)
	{
		CollectionName = collectionName;
	}

	public TenantRepositoryAttribute(string collectionName, int schemaVersion)
	{
		CollectionName = collectionName;
		SchemaVersion = schemaVersion;
	}
}
