using System.Linq.Expressions;
using Azure;
using Azure.Data.Tables;

namespace TGC.AzureTableStorage.Tests;

/// <summary>
/// Mock implementation which can be used for unit tests and mocked setups.
/// </summary>
/// <typeparam name="T">Type implementing ITableEntity</typeparam>
public class StubbedAzureStableStorageRepository<T> : IAzureTableStorageRepository<T> where T : class, ITableEntity, new()
{
	public Task<Response> CreateAsync(T tableEntity)
	{
		throw new NotImplementedException();
	}

	public Pageable<T> Query(Expression<Func<T, bool>> filter, int? maxPerPage = null, IEnumerable<string>? select = null, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public AsyncPageable<T> QueryAsync(Expression<Func<T, bool>> filter, int? maxPerPage = null, IEnumerable<string>? select = null, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<T> GetSingleAsync(Expression<Func<T, bool>> filter)
	{
		throw new NotImplementedException();
	}

	public Task<T> GetSinglePropertiesAsync(Expression<Func<T, bool>> filter, IEnumerable<string> select)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter)
	{
		throw new NotImplementedException();
	}

	public Task<Guid> DeleteAsync(T tableEntity)
	{
		throw new NotImplementedException();
	}

	public Task<Guid> DeleteByIdAsync(Guid id)
	{
		throw new NotImplementedException();
	}

	public Task<bool> ExistsByIdAsync(Guid id)
	{
		throw new NotImplementedException();
	}

	public Task<bool> ExistsAsync(Expression<Func<T, bool>> filter)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<T>> GetAllWithPropertiesAsync(Expression<Func<T, bool>> filter, IEnumerable<string> select)
	{
		throw new NotImplementedException();
	}
}
