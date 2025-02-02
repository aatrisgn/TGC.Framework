using System.Linq.Expressions;
using Azure;
using Azure.Data.Tables;

namespace TGC.AzureTableStorage;

/// <summary>
/// Interface for Azure Table Storage repository operations.
/// </summary>
/// <typeparam name="T">The type of entity, which must implement ITableEntity and have a parameterless constructor.</typeparam>
public interface IAzureTableStorageRepository<T> where T : class, ITableEntity, new()
{
	/// <summary>
	/// Creates a new entity in the table storage.
	/// </summary>
	/// <param name="tableEntity">The entity to create.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the response from Azure Table Storage.</returns>
	Task<Response> CreateAsync(T tableEntity);

	/// <summary>
	/// Queries the table storage for entities that match the specified filter.
	/// </summary>
	/// <param name="filter">The filter expression to match entities.</param>
	/// <param name="maxPerPage">The maximum number of items per page. Optional and defaults to 1.000</param>
	/// <param name="select">The properties to select.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A pageable collection of entities that match the filter.</returns>
	Pageable<T> Query(Expression<Func<T, bool>> filter, int? maxPerPage = null, IEnumerable<string>? select = null, CancellationToken cancellationToken = default);

	/// <summary>
	/// Asynchronously queries the table storage for entities that match the specified filter.
	/// </summary>
	/// <param name="filter">The filter expression to match entities.</param>
	/// <param name="maxPerPage">The maximum number of items per page. Optional and defaults to 1.000</param>
	/// <param name="select">The properties to select.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>An async pageable collection of entities that match the filter.</returns>
	AsyncPageable<T> QueryAsync(Expression<Func<T, bool>> filter, int? maxPerPage = null, IEnumerable<string>? select = null, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets a single entity that matches the specified filter.
	/// </summary>
	/// <param name="filter">The filter expression to match entities.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the single entity that matches the filter.</returns>
	/// <exception cref="InvalidOperationException">Thrown if no entities or more than one entity matches the filter.</exception>
	Task<T> GetSingleAsync(Expression<Func<T, bool>> filter);

	Task<T> GetSinglePropertiesAsync(Expression<Func<T, bool>> filter, IEnumerable<string> select);

	/// <summary>
	/// Gets all entities that match the specified filter.
	/// </summary>
	/// <param name="filter">The filter expression to match entities.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities that match the filter.</returns>
	Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter);
	Task<IEnumerable<T>> GetAllWithPropertiesAsync(Expression<Func<T, bool>> filter, IEnumerable<string> select);
}
