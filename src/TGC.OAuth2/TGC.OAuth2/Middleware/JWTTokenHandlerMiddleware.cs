using Microsoft.AspNetCore.Http;
using TGC.OAuth2.Abstractions;

namespace TGC.OAuth2.Middleware;
internal class JWTTokenHandlerMiddleware : IAuthMiddleware
{
	private readonly RequestDelegate _next;

	public JWTTokenHandlerMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context, IPrincipalContext principalContext)
	{
		principalContext.SetBearerToken(context.Request.Headers);
		await _next(context);
	}
}
