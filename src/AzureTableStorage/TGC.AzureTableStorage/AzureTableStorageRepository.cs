using System.Linq.Expressions;
using Azure;
using Azure.Data.Tables;
using TGC.AzureTableStorage.Extensions;

namespace TGC.AzureTableStorage;

public class AzureTableStorageRepository<T> : IAzureTableStorageRepository<T> where T : class, ITableEntity, new()
{
	private readonly ITableClientFactory _tableClientFactory;
	private readonly string _tableName;

	private string PartitionKey;

	public AzureTableStorageRepository(ITableClientFactory tableClientFactory)
	{
		_tableClientFactory = tableClientFactory;

		var repositoryItemType = typeof(T);
		var customAttributes = repositoryItemType.GetCustomAttributes(typeof(TableItemAttribute), false);

		if (customAttributes.Length == 0)
		{
			throw new InvalidOperationException("No table item attribute found. Ensure TableItemAttribute has been defined on item.");
		}

		var customAttribute = (TableItemAttribute)customAttributes.First();

		this.PartitionKey = string.IsNullOrEmpty(customAttribute.PartitionKey) ? customAttribute.TableName : customAttribute.PartitionKey;
		this._tableName = customAttribute.TableName;
	}

	public async Task<Response> CreateAsync(T tableEntity)
	{
		ArgumentNullException.ThrowIfNull(tableEntity);

		var tableClient = GetClient();

		tableEntity.PartitionKey = this.PartitionKey;

		return await tableClient.AddEntityAsync(tableEntity);
	}

	public AsyncPageable<T> QueryAsync(Expression<Func<T, bool>> filter, int? maxPerPage = default, IEnumerable<string>? select = default, CancellationToken cancellationToken = default)
	{
		var tableClient = GetClient();
		return tableClient.QueryAsync(filter, maxPerPage, select, cancellationToken);
	}

	/// <summary>
	/// Get all items which qualifis for the filter expression. Note: A maximum of 1.000 results are returned.
	/// </summary>
	/// <param name="filter">Expression to match entities for.</param>
	/// <returns>IEnumerable containing matched entities.</returns>
	public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter)
	{
		var tableClient = GetClient();
		var asyncPageable = tableClient.QueryAsync(filter, null, GetAllTPropertyNames());

		return await asyncPageable.AsIEnumerableAsync();
	}

	public async Task<IEnumerable<T>> GetAllWithPropertiesAsync(Expression<Func<T, bool>> filter, IEnumerable<string> select)
	{
		var tableClient = GetClient();
		var asyncPageable = tableClient.QueryAsync(filter, null, select);

		return await asyncPageable.AsIEnumerableAsync();
	}

	/// <summary>
	/// Get single item matching filter. Will throw exception if 0 or more than 1 entity is located.
	/// </summary>
	/// <param name="filter">Expression to match entities for.</param>
	/// <returns>IEnumerable containing matched entities.</returns>
	public async Task<T> GetSingleAsync(Expression<Func<T, bool>> filter)
	{
		var tableClient = GetClient();
		var asyncPageable = tableClient.QueryAsync(filter, null, GetAllTPropertyNames());

		return await asyncPageable.SingleAsync();
	}

	public async Task<T> GetSinglePropertiesAsync(Expression<Func<T, bool>> filter, IEnumerable<string> select)
	{
		var tableClient = GetClient();
		var asyncPageable = tableClient.QueryAsync(filter, null, select);

		return await asyncPageable.SingleAsync();
	}

	public Pageable<T> Query(Expression<Func<T, bool>> filter, int? maxPerPage = default, IEnumerable<string>? select = default, CancellationToken cancellationToken = default)
	{
		var tableClient = GetClient();
		return tableClient.Query(filter, maxPerPage, select, cancellationToken);
	}

	private TableClient GetClient()
	{
		return _tableClientFactory.GetClient(_tableName);
	}

	private static List<string> GetAllTPropertyNames()
	{
		var propertyNames = typeof(T).GetProperties()
			.Select(prop => prop.Name)
			.ToList();
		return propertyNames;
	}
}
