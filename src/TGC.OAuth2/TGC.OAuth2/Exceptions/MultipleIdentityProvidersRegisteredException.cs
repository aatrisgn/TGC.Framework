namespace TGC.OAuth2.Exceptions;
internal class MultipleIdentityProvidersRegisteredException : Exception
{
	public MultipleIdentityProvidersRegisteredException() : base()
	{
	}

	public MultipleIdentityProvidersRegisteredException(string? message) : base(message)
	{
	}

	public MultipleIdentityProvidersRegisteredException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
