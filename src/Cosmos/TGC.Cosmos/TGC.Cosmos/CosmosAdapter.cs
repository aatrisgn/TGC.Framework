using System.Linq.Expressions;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using TGC.Cosmos.Abstractions;
using TGC.Cosmos.Models;

namespace TGC.Cosmos;
internal class CosmosAdapter<T> : ICosmosAdapter<T> where T : RepositoryEntity
{
	private readonly ICosmosClientConnectionFactory _cosmosClientConnectionFactory;
	public CosmosAdapter(ICosmosClientConnectionFactory cosmosClientConnectionFactory)
	{
		_cosmosClientConnectionFactory = cosmosClientConnectionFactory;
	}

	public async Task<T> CreateAsync(T entity)
	{
		var container = await _cosmosClientConnectionFactory.GetContainerAsync<T>();
		var result = await container.CreateItemAsync(entity);
		return result.Resource;
	}

	public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression)
	{
		throw new NotImplementedException();
	}

	public async IAsyncEnumerable<T> GetAllAsStream()
	{
		var container = await _cosmosClientConnectionFactory.GetContainerAsync<T>();
		var queryable = container.GetItemLinqQueryable<T>();

		using var feed = queryable.ToFeedIterator();
		while (feed.HasMoreResults)
		{
			FeedResponse<T> response = await feed.ReadNextAsync();

			foreach (var item in response)
			{
				yield return item;
			}
		}
	}

	public Task<T> GetByIdAsync(string id)
	{
		throw new NotImplementedException();
	}

	public async Task<IEnumerable<T>> GetAllAsync()
	{
		var container = await _cosmosClientConnectionFactory.GetContainerAsync<T>();

		var q = container.GetItemLinqQueryable<T>();
		var iterator = q.ToFeedIterator();
		var results = await iterator.ReadNextAsync();
		return [.. results];
	}
}
