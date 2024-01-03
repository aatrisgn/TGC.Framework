using Microsoft.AspNetCore.Mvc;
using TGC.Common.Exceptions;

namespace TGC.Core.TestAPI;

public class ExceptionDescriptor1 : IExceptionDescriptor
{
	public bool AcceptsException(Exception exception)
	{
		return exception.GetType() == typeof(NotImplementedException);
	}

	public Task<ProblemDetails> HandleException(Exception exception)
	{
		var problemDetails = new ProblemDetails
		{
			Status = 404,
			Instance = "about:blank",
			Title = exception.GetType().Name,
			Type = exception.Message
		};

		return Task.FromResult(problemDetails);
	}
}
