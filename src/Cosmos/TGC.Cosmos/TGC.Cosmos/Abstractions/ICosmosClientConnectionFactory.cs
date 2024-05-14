using Microsoft.Azure.Cosmos;

namespace TGC.Cosmos.Abstractions;
public interface ICosmosClientConnectionFactory
{
	Task<Container> GetContainerAsync<T>();
	CosmosClient GetClient();
}
