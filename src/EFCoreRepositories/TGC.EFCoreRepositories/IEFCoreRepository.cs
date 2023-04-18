using System.Linq.Expressions;

namespace TGC.EFCoreRepositories;
public interface IEFCoreRepository<T> where T : class, IEFCoreDTO, new()
{
	Task<T> GetByIdAsync(Guid guid);
	Task<T> CreateAsync(T entity);
	Task<T> UpdateAsync(T entity);

	/// <summary>
	/// Locates and deactivates an entity by changing 'Active' to false.
	/// </summary>
	/// <param name="id">Id of entity to be deactivated.</param>
	/// <returns></returns>
	Task SoftDeleteAsync(Guid id);

	/// <summary>
	/// Locates and deactivates an entity by changing 'Active' to false.
	/// </summary>
	/// <param name="entity">Entity to be deactivated.</param>
	/// <returns></returns>
	Task SoftDeleteAsync(T entity);

	/// <summary>
	/// Locates and removes an entity from the database. Cannot be reverted.
	/// </summary>
	/// <param name="id">Id of entity to remove</param>
	/// <returns></returns>
	Task HardDeleteAsync(Guid id);

	/// <summary>
	/// Locates and removes an entity from the database. Cannot be reverted.
	/// </summary>
	/// <param name="entity">Entity to remove</param>
	/// <returns></returns>
	Task HardDeleteAsync(T entity);

	/// <summary>
	/// Checks whether an entity exist with a given Id.
	/// </summary>
	/// <param name="guid">Id of entity to check whether exists.</param>
	/// <returns></returns>
	Task<bool> ExistsAsync(Guid guid);
	Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
	Task<List<Guid>> GetAllIdsAsync();
	Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> expression);
	Task<T> FindSingleAsync(Expression<Func<T, bool>> expression);
	Task<T> FindFirstAsync(Expression<Func<T, bool>> expression);
}
