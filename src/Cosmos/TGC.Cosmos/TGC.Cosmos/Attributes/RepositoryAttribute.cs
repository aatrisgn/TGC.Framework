namespace TGC.Cosmos.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class RepositoryAttribute : Attribute
{
	public string CollectionName { get; }
	public int SchemaVersion { get; }

	public RepositoryAttribute(string collectionName)
	{
		CollectionName = collectionName;
	}

	public RepositoryAttribute(string collectionName, int schemaVersion)
	{
		CollectionName = collectionName;
		SchemaVersion = schemaVersion;
	}
}
