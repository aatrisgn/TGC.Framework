namespace TGC.Cosmos.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class RepositoryEntityAttribute : Attribute
{
	public string CollectionName { get; }
	public int SchemaVersion { get; }

	public RepositoryEntityAttribute(string collectionName)
	{
		CollectionName = collectionName;
	}

	public RepositoryEntityAttribute(string collectionName, int schemaVersion)
	{
		CollectionName = collectionName;
		SchemaVersion = schemaVersion;
	}
}
