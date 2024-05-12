using System.Linq.Expressions;
using TGC.Cosmos.Models;

namespace TGC.Cosmos.Abstractions;
public interface ICosmosRepository<T> where T : RepositoryEntity
{
	Task<T> CreateAsync(T entity);
	Task<T> GetByIdAsync(string id);
	Task<IEnumerable<T>> GetAllAsync();
	Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression);
	Task<T> GetSingleAsync(Expression<Func<T, bool>> expression);
}
