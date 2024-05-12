using Microsoft.Azure.Cosmos;

namespace TGC.Cosmos.Abstractions;
public interface ICosmosClientConnectionFactory
{
	Container GetContainer<T>();
	CosmosClient GetClient();
}
