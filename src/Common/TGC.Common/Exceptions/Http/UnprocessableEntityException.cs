namespace TGC.Common.Exceptions.Http;

[Serializable]
public class UnprocessableEntityException : Exception
{
	public UnprocessableEntityException() : base("Entity was unprocessable.")
	{
	}

	public UnprocessableEntityException(string? message) : base(message)
	{
	}

	public UnprocessableEntityException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
