namespace TGC.Common.Exceptions.Http;
[Serializable]
public class NotImplementedException : Exception
{
	public NotImplementedException() : base("Endpoint / Method is not imeplemented in the API.")
	{
	}

	public NotImplementedException(string? message) : base(message)
	{
	}

	public NotImplementedException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
