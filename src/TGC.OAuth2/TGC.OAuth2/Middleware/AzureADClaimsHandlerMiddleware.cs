using Microsoft.AspNetCore.Http;
using TGC.OAuth2.Abstractions;

namespace TGC.OAuth2.Middleware;

internal class AzureADClaimsHandlerMiddleware : IAuthMiddleware
{
	private readonly RequestDelegate _next;

	public AzureADClaimsHandlerMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context, IPrincipalContext principalContext)
	{
		principalContext.SetPrincipalContext(context.User);
		await _next(context);
	}
}
