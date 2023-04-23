namespace TGC.Configuration.Exceptions;
internal class TypedConfigurationNotFoundException : Exception
{
	public TypedConfigurationNotFoundException() : base()
	{
	}

	public TypedConfigurationNotFoundException(string? message) : base(message)
	{
	}

	public TypedConfigurationNotFoundException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
