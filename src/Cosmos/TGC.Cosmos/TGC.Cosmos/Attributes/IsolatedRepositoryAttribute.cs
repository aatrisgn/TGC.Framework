namespace TGC.Cosmos.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class IsolatedRepositoryAttribute : Attribute
{
	public string CollectionName { get; }
	public int SchemaVersion { get; }

	public IsolatedRepositoryAttribute(string collectionName)
	{
		CollectionName = collectionName;
	}

	public IsolatedRepositoryAttribute(string collectionName, int schemaVersion)
	{
		CollectionName = collectionName;
		SchemaVersion = schemaVersion;
	}
}
