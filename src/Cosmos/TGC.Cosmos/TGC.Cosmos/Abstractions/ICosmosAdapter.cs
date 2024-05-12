using System.Linq.Expressions;
using TGC.Cosmos.Models;

namespace TGC.Cosmos.Abstractions;
internal interface ICosmosAdapter<T> where T : RepositoryEntity
{
	Task<T> CreateAsync(T entity);
	Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression);
	Task<IEnumerable<T>> GetAllAsync();
	IAsyncEnumerable<T> GetAllAsStream();
	Task<T> GetByIdAsync(string id);
}
