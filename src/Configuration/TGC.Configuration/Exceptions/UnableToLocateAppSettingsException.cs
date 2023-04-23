namespace TGC.Configuration.Exceptions;
internal class UnableToLocateAppSettingsException : Exception
{
	public UnableToLocateAppSettingsException() : base()
	{
	}

	public UnableToLocateAppSettingsException(string? message) : base(message)
	{
	}

	public UnableToLocateAppSettingsException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
