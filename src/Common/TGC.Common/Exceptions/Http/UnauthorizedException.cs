namespace TGC.Common.Exceptions.Http;
[Serializable]
public class UnauthorizedException : Exception
{
	public UnauthorizedException() : base("Unauthorized access.")
	{
	}

	public UnauthorizedException(string? message) : base(message)
	{
	}

	public UnauthorizedException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
