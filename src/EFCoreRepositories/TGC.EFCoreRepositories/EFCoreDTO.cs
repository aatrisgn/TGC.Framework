using System.ComponentModel.DataAnnotations;

namespace TGC.EFCoreRepositories;
public class EFCoreDTO : IEFCoreDTO
{
	/// <summary>
	/// Unique identifier for entity
	/// </summary>
	[Key]
	public Guid Id { get; set; }

	/// <summary>
	/// DateTime defining when the entity was created
	/// </summary>
	public DateTime Created { get; set; }

	/// <summary>
	/// DateTime defining when the entity was last updated
	/// </summary>
	public DateTime LastUpdated { get; set; }

	/// <summary>
	/// Defines whether entity is active or not
	/// </summary>
	public bool Active { get; set; }
}
