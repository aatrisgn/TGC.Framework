namespace TGC.Common.Exceptions.Http;

[Serializable]
public class ForbiddenException : Exception
{
	public ForbiddenException() : base("Request is forbidden.")
	{
	}

	public ForbiddenException(string? message) : base(message)
	{
	}

	public ForbiddenException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
