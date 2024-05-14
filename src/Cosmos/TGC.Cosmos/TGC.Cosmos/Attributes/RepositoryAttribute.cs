namespace TGC.Cosmos.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class RepositoryAttribute : Attribute
{
	public int SchemaVersion { get; }

	public RepositoryAttribute(int schemaVersion)
	{
		SchemaVersion = schemaVersion;
	}
}
