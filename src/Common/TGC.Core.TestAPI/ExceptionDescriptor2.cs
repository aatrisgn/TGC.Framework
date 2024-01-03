using Microsoft.AspNetCore.Mvc;
using TGC.Core.Exceptions.Abstractions;

namespace TGC.Core.TestAPI;

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
