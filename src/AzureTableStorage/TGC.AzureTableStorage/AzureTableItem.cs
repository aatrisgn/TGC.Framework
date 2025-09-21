using Azure;
using Azure.Data.Tables;

namespace TGC.AzureTableStorage;

/// <summary>
/// Represents a base entity for Azure Table Storage, implementing <see cref="ITableEntity"/>.
/// </summary>
public abstract class AzureTableItem : ITableEntity
{
	/// <summary>
	/// Gets or sets the partition key for the entity.
	/// </summary>
	public string? PartitionKey { get; set; }

	/// <summary>
	/// Gets or sets the row key for the entity. Defaults to a new GUID string.
	/// </summary>
	public string RowKey { get; set; } = Guid.NewGuid().ToString();

	/// <summary>
	/// Gets or sets the timestamp for the entity, maintained by Azure Table Storage.
	/// </summary>
	public DateTimeOffset? Timestamp { get; set; }

	/// <summary>
	/// Gets or sets the ETag for the entity, used for concurrency checks.
	/// </summary>
	public ETag ETag { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the entity is active.
	/// </summary>
	public bool IsActive { get; set; }
}
