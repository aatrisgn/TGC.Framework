namespace TGC.ConsoleBuilder.Exceptions;
internal class NoAsyncStartServiceFoundException : Exception
{
	public NoAsyncStartServiceFoundException() : base()
	{
	}

	public NoAsyncStartServiceFoundException(string? message) : base(message)
	{
	}

	public NoAsyncStartServiceFoundException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
