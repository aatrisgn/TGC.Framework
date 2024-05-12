using TGC.Cosmos.Models;

namespace TGC.Cosmos.Abstractions;
internal interface ICosmosTenantRepository<T> : ICosmosRepository<T> where T : TenantRepositoryEntity
{
}
