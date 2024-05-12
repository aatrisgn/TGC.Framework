using Microsoft.Extensions.DependencyInjection;
using TGC.Cosmos.Abstractions;
using TGC.Cosmos.Repositories;

namespace TGC.Cosmos.Extensions;
public static class IServiceCollectionExtensions
{
	public static IServiceCollection ConfigureCosmos(this IServiceCollection services, Action<CosmosConfiguration> action)
	{
		ArgumentNullException.ThrowIfNull(action);

		var cosmosConfiguration = new CosmosConfiguration
		{
			DatabaseName = string.Empty
		};

		action.Invoke(cosmosConfiguration);

		cosmosConfiguration.ConnectionString = Environment.GetEnvironmentVariable(Constants.CONNECTIONSTRING_ENVIRONMENTVARIABLENAME);

		services.AddSingleton<ICosmosConfiguration>(cosmosConfiguration);

		if (cosmosConfiguration.Enabled)
		{
			services.ConfigureCosmos();
		}
		else
		{
			services.ConfigureCosmosInMemory();
		}

		return services;
	}

	private static IServiceCollection ConfigureCosmos(this IServiceCollection services)
	{
		services.AddSingleton<ICosmosClientConnectionFactory, CosmosClientConnectionFactory>();
		services.AddScoped(typeof(ICosmosAdapter<>), typeof(CosmosAdapter<>));
		services.AddScoped(typeof(ICosmosRepository<>), typeof(CosmosRepository<>));
		services.AddScoped(typeof(ICosmosTenantRepository<>), typeof(CosmosTenantRepository<>));
		return services;
	}

	private static IServiceCollection ConfigureCosmosInMemory(this IServiceCollection services)
	{
		return services;
	}
}
