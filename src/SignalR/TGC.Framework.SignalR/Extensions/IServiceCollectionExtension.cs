using Microsoft.Extensions.DependencyInjection;
using TGC.Framework.SignalR.Abstractions;

namespace TGC.Framework.SignalR.Extensions;

/// <summary>
/// Extensions for <see cref="IServiceCollection"/> to configure TGC.Framework.SignalR service
/// </summary>
public static class IServiceCollectionExtension
{
	/// <summary>
	/// Configures SignalR services for the given <see cref="IServiceCollection"/>.
	/// </summary>
	/// <param name="serviceCollection">The <see cref="IServiceCollection"/> to configure.</param>
	/// <returns>The configured <see cref="IServiceCollection"/>.</returns>
	public static IServiceCollection ConfigureSignalR(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSignalR();
		serviceCollection.AddScoped<ISignalRConnectionContext, SignalRConnectionContext>();
		serviceCollection.AddScoped<ISignalRService, SignalRService>();
		return serviceCollection;
	}
}
