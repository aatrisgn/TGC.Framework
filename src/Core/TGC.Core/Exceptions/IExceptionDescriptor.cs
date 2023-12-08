using Microsoft.AspNetCore.Mvc;

namespace TGC.Core.Exceptions.Abstractions;
public interface IExceptionDescriptor
{
	bool AcceptsException(Exception exception);
	Task<ProblemDetails> HandleException(Exception exception);
}
