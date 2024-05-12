namespace TGC.Cosmos.Models;
public class RepositoryEntity
{
	public required string Id { get; set; }
	public string? ETag { get; set; }
	public DateTime Created { get; set; }
	public DateTime LastEdited { get; set; }
}
