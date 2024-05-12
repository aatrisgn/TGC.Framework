using System.Linq.Expressions;
using TGC.Cosmos.Abstractions;
using TGC.Cosmos.Models;

namespace TGC.Cosmos.Repositories;
internal class CosmosRepository<T> : ICosmosRepository<T> where T : RepositoryEntity
{
	private readonly ICosmosAdapter<T> _cosmosAdapter;

	public CosmosRepository(ICosmosAdapter<T> cosmosAdapter)
	{
		_cosmosAdapter = cosmosAdapter;
	}

	public async Task<T> CreateAsync(T entity)
	{
		return await _cosmosAdapter.CreateAsync(entity);
	}

	public async Task<IEnumerable<T>> GetAllAsync()
	{
		return await _cosmosAdapter.GetAllAsync();
	}

	public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression)
	{
		return await _cosmosAdapter.GetAllAsync(expression);
	}

	public async Task<T> GetByIdAsync(string id)
	{
		return await _cosmosAdapter.GetByIdAsync(id);
	}

	public async Task<T> GetSingleAsync(Expression<Func<T, bool>> expression)
	{
		var locatedEntities = await _cosmosAdapter.GetAllAsync(expression);
		return locatedEntities.Single();
	}
}
