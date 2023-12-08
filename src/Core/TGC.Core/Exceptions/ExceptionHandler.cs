using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TGC.Core.Exceptions.Abstractions;

namespace TGC.Core.Exceptions;
internal class ExceptionHandler : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		IEnumerable<IExceptionDescriptor> exceptionDescriptors = httpContext.RequestServices.GetServices<IExceptionDescriptor>();

		var specificExceptionDescriptor = exceptionDescriptors.SingleOrDefault(e => e.AcceptsException(exception));

		if (specificExceptionDescriptor != null)
		{
			var problemDetails = await specificExceptionDescriptor.HandleException(exception);
			await httpContext.Response.WriteAsJsonAsync(problemDetails);
		}
		else
		{
			await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
			{
				Status = (int)HttpStatusCode.InternalServerError,
				Type = exception.GetType().Name,
				Title = "An unexpected error occurred",
				Detail = exception.Message,
				Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
			});
		}

		return true;
	}
}
