using Microsoft.AspNetCore.Mvc;

namespace TGC.Common.Exceptions;
public interface IExceptionDescriptor
{
	bool AcceptsException(Exception exception);
	Task<ProblemDetails> HandleException(Exception exception);
}
