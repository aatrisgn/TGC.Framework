using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace TGC.OpenAPISpecGeneration.IoC;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddOpenAPISpecDefinition(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddEndpointsApiExplorer();
		serviceCollection.AddSwaggerGen();
		return serviceCollection;
	}
}
