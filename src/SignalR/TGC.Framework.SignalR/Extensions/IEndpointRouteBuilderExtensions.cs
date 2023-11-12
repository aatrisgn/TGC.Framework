using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace TGC.Framework.SignalR.Extensions;

public static class IEndpointRouteBuilderExtensions
{
	public static IEndpointRouteBuilder ConfigureSignalREndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
	{
		endpointRouteBuilder.MapHub<SignalRHub>("signalr");
		return endpointRouteBuilder;
	}
}
