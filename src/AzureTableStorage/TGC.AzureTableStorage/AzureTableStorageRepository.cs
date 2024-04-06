using System.Linq.Expressions;
using Azure;
using Azure.Data.Tables;

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
		var customAttribute = (TableItemAttribute)repositoryItemType.GetCustomAttributes(typeof(TableItemAttribute), false).First();

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

	public Pageable<T> Query(Expression<Func<T, bool>> filter, int? maxPerPage = default, IEnumerable<string>? select = default, CancellationToken cancellationToken = default)
	{
		var tableClient = GetClient();
		return tableClient.Query(filter, maxPerPage, select, cancellationToken);
	}

	private TableClient GetClient()
	{
		return _tableClientFactory.GetClient(_tableName);
	}
}
