using System.Linq.Expressions;
using Azure;
using Azure.Data.Tables;

namespace TGC.AzureTableStorage.Tests;

/// <summary>
/// Mock implementation which can be used for unit tests and mocked setups.
/// </summary>
/// <typeparam name="T">Type implementing ITableEntity</typeparam>
internal class StubbedAzureStableStorageRepository<T> : IAzureTableStorageRepository<T> where T : class, ITableEntity, new()
{
	private Dictionary<Guid, T> tableEntities = new();
	private string partitionKey;

	public StubbedAzureStableStorageRepository()
	{
		var repositoryItemType = typeof(T);
		var customAttributes = repositoryItemType.GetCustomAttributes(typeof(TableItemAttribute), false);

		if (customAttributes.Length == 0)
		{
			throw new InvalidOperationException("No table item attribute found. Ensure TableItemAttribute has been defined on item.");
		}

		var customAttribute = (TableItemAttribute)customAttributes.First();

		partitionKey = string.IsNullOrEmpty(customAttribute.PartitionKey) ? customAttribute.TableName : customAttribute.PartitionKey;
	}

	public Task<Response> CreateAsync(T tableEntity)
	{
		ArgumentNullException.ThrowIfNull(tableEntity);

		var rowKey = Guid.NewGuid();
		tableEntity.RowKey = rowKey.ToString();
		tableEntity.PartitionKey = partitionKey;

		var entityCreated = tableEntities.TryAdd(rowKey, tableEntity);

		MockResponse mockResponse;

		if (!entityCreated)
		{
			mockResponse = MockResponse.Create409Conflict();
		}
		else
		{
			mockResponse = MockResponse.Create200OK();
		}

		return Task.FromResult((Response)mockResponse);
	}

	public Pageable<T> Query(Expression<Func<T, bool>> filter, int? maxPerPage = null, IEnumerable<string>? select = null, CancellationToken cancellationToken = default)
	{
		if (cancellationToken != default || select != null)
		{
			throw new NotImplementedException("Specifying 'select' or 'cancellationToken' is currently not supported in testing setup.");
		}

		var compiledExpression = filter.Compile();
		var locatedValue = tableEntities.Values.Where(compiledExpression);

		return new MockPageable<T>(locatedValue);
	}

	public AsyncPageable<T> QueryAsync(Expression<Func<T, bool>> filter, int? maxPerPage = null, IEnumerable<string>? select = null, CancellationToken cancellationToken = default)
	{
		if (cancellationToken != default || select != null)
		{
			throw new NotImplementedException("Specifying 'select' or 'cancellationToken' is currently not supported in testing setup.");
		}

		var compiledExpression = filter.Compile();
		var locatedValue = tableEntities.Values.Where(compiledExpression);

		return new MockAsyncPageable<T>(locatedValue);
	}

	public Task<T> GetSingleAsync(Expression<Func<T, bool>> filter)
	{
		ArgumentNullException.ThrowIfNull(filter);

		var compiledExpression = filter.Compile();
		var locatedValue = tableEntities.Values.Single(compiledExpression);

		return Task.FromResult(locatedValue);
	}

	public Task<T> GetSinglePropertiesAsync(Expression<Func<T, bool>> filter, IEnumerable<string> select)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter)
	{
		ArgumentNullException.ThrowIfNull(filter);

		var compiledExpression = filter.Compile();
		var locatedValues = tableEntities.Values.Where(compiledExpression).AsEnumerable();

		return Task.FromResult(locatedValues);
	}

	public Task<Guid> DeleteAsync(T tableEntity)
	{
		ArgumentNullException.ThrowIfNull(tableEntity);

		var relevantRowKey = Guid.Parse(tableEntity.RowKey);

		tableEntities.Remove(relevantRowKey);

		return Task.FromResult(relevantRowKey);
	}

	public Task<Guid> DeleteByIdAsync(Guid rowKey)
	{
		ArgumentNullException.ThrowIfNull(rowKey);

		tableEntities.Remove(rowKey);

		return Task.FromResult(rowKey);
	}

	public Task<bool> ExistsByIdAsync(Guid rowKey)
	{
		ArgumentNullException.ThrowIfNull(rowKey);

		var foundItemByRowKey = tableEntities.TryGetValue(rowKey, out _);

		return Task.FromResult(foundItemByRowKey);
	}

	public Task<bool> ExistsAsync(Expression<Func<T, bool>> filter)
	{
		ArgumentNullException.ThrowIfNull(filter);

		var compiledExpression = filter.Compile();
		var locatedValues = tableEntities.Values.SingleOrDefault(compiledExpression);

		return locatedValues is null ? Task.FromResult(false) : Task.FromResult(true);
	}

	public Task<IEnumerable<T>> GetAllWithPropertiesAsync(Expression<Func<T, bool>> filter, IEnumerable<string> select)
	{
		throw new NotImplementedException();
	}
}
