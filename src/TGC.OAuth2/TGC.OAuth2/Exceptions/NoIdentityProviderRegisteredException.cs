namespace TGC.OAuth2.Exceptions;
internal class NoIdentityProviderRegisteredException : Exception
{
	public NoIdentityProviderRegisteredException() : base()
	{
	}

	public NoIdentityProviderRegisteredException(string? message) : base(message)
	{
	}

	public NoIdentityProviderRegisteredException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
