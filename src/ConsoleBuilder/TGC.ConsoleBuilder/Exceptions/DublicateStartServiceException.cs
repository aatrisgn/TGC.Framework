namespace TGC.ConsoleBuilder.Exceptions;
internal class DublicateStartServiceException : Exception
{
	public DublicateStartServiceException() : base()
	{
	}

	public DublicateStartServiceException(string? message) : base(message)
	{
	}

	public DublicateStartServiceException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
