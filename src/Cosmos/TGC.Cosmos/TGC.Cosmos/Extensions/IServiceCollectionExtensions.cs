using Microsoft.Extensions.DependencyInjection;

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

		services.AddSingleton<ICosmosConfiguration>(cosmosConfiguration);

		return services;
	}
}
