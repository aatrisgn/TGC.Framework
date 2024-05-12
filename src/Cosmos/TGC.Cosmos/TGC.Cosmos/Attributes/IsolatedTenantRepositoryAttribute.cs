namespace TGC.Cosmos.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class IsolatedTenantRepositoryAttribute : Attribute
{
	public string CollectionName { get; }
	public int SchemaVersion { get; }

	public IsolatedTenantRepositoryAttribute(string collectionName)
	{
		CollectionName = collectionName;
	}

	public IsolatedTenantRepositoryAttribute(string collectionName, int schemaVersion)
	{
		CollectionName = collectionName;
		SchemaVersion = schemaVersion;
	}
}
