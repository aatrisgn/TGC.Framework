using Microsoft.AspNetCore.Mvc;
using TGC.Common.Exceptions;

namespace TGC.Common.TestAPI;

public class ExceptionDescriptor2 : IExceptionDescriptor
{
	public bool AcceptsException(Exception exception)
	{
		return exception.GetType() == typeof(KeyNotFoundException);
	}

	public Task<ProblemDetails> HandleException(Exception exception)
	{
		var problemDetails = new ProblemDetails
		{
			Status = 400,
			Instance = "about:blank",
			Title = exception.GetType().Name,
			Type = exception.Message
		};

		return Task.FromResult(problemDetails);
	}
}
