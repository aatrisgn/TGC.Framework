using System.Linq.Expressions;
using Azure;
using Azure.Data.Tables;

namespace TGC.AzureTableStorage;

public class AzureTableStorageRepository<T> : IAzureTableStorageRepository<T> where T : class, ITableEntity, new()
{
	private readonly ITableStorageContext _tableStorageContext;

	private TableClient tableClient;

	private string PartitionKey;

	public AzureTableStorageRepository(ITableStorageContext tableStorageContext)
	{
		_tableStorageContext = tableStorageContext;
		var repositoryItemType = typeof(T);
		var customAttribute = (TableItemAttribute)repositoryItemType.GetCustomAttributes(typeof(TableItemAttribute), false).First();

		this.PartitionKey = string.IsNullOrEmpty(customAttribute.PartitionKey) ? customAttribute.TableName : customAttribute.PartitionKey;

		tableClient = _tableStorageContext.CreateClient(customAttribute.TableName);
		tableClient.CreateIfNotExists();
	}

	public async Task<Response> CreateAsync(T tableEntity)
	{
		tableEntity.PartitionKey = this.PartitionKey;
		return await tableClient.AddEntityAsync(tableEntity);
	}

	public AsyncPageable<T> QueryAsync(Expression<Func<T, bool>> filter, int? maxPerPage = default, IEnumerable<string>? select = default, CancellationToken cancellationToken = default)
	{
		return tableClient.QueryAsync(filter, maxPerPage, select, cancellationToken);
	}

	public Pageable<T> Query(Expression<Func<T, bool>> filter, int? maxPerPage = default, IEnumerable<string>? select = default, CancellationToken cancellationToken = default)
	{
		return tableClient.Query(filter, maxPerPage, select, cancellationToken);
	}
}
