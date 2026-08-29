using Microsoft.Extensions.DependencyInjection;
using TGC.Communication.cqrs;

namespace TGC.Communication;

public static class IServiceCollectionExtensions
{
	public static IServiceCollection RegisterMediator(this IServiceCollection services)
	{
		services.AddScoped<IMediator, Mediator>();
		return services;
	}
}