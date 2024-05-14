using Newtonsoft.Json;

namespace TGC.Cosmos.Models;
public class RepositoryEntity
{
	[JsonProperty(PropertyName = "id")]
	public string Id { get; init; }
	[JsonProperty(PropertyName = "_etag")]
	public string? ETag { get; set; }
	public string CollectionType => GetType().Name;
	public int SchemaVersion { get; set; }
	public DateTime Created { get; set; }
	public DateTime LastEdited { get; set; }

	public RepositoryEntity()
	{
		if (string.IsNullOrEmpty(Id))
		{
			Id = Guid.NewGuid().ToString();
		}
	}
}
