namespace TGC.Common.Exceptions.Http;
[Serializable]
public class GoneException : Exception
{
	public GoneException() : base("Endpoint does not exist.")
	{
	}

	public GoneException(string? message) : base(message)
	{
	}

	public GoneException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
