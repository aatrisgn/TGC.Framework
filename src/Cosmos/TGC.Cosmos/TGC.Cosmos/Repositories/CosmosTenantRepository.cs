using System.Linq.Expressions;
using TGC.Cosmos.Abstractions;
using TGC.Cosmos.Models;

namespace TGC.Cosmos.Repositories;
internal class CosmosTenantRepository<T> : ICosmosTenantRepository<T> where T : TenantRepositoryEntity
{
	public Task<T> CreateAsync(T entity)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<T>> GetAllAsync()
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression)
	{
		throw new NotImplementedException();
	}

	public Task<T> GetByIdAsync(string id)
	{
		throw new NotImplementedException();
	}

	public Task<T> GetSingleAsync(Expression<Func<T, bool>> expression)
	{
		throw new NotImplementedException();
	}
}
