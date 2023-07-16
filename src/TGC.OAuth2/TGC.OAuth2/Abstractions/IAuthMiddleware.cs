using Microsoft.AspNetCore.Http;

namespace TGC.OAuth2.Abstractions;
internal interface IAuthMiddleware
{
	Task InvokeAsync(HttpContext context, IPrincipalContext principalContext);
}
