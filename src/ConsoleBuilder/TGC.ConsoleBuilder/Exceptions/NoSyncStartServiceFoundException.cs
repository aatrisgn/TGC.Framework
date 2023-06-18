namespace TGC.ConsoleBuilder.Exceptions;
internal class NoSyncStartServiceFoundException : Exception
{
	public NoSyncStartServiceFoundException() : base()
	{
	}

	public NoSyncStartServiceFoundException(string? message) : base(message)
	{
	}

	public NoSyncStartServiceFoundException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
