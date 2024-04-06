using Microsoft.AspNetCore.Mvc;

namespace TGC.Common.Exceptions;
internal abstract class BaseExceptionHandler : IExceptionDescriptor
{
	public abstract Type ExceptionHandled { get; }
	public abstract int StatusCode { get; }
	public abstract string Title { get; }
	public string? Details { get; }

	public bool AcceptsException(Exception exception)
	{
		return exception.GetType() == ExceptionHandled;
	}

	public Task<ProblemDetails> HandleException(Exception exception)
	{
		return Task.FromResult(new ProblemDetails
		{
			Status = StatusCode,
			Type = "about:blank",
			Title = Title,
			Detail = Details ?? exception.Message
		});
	}
}
