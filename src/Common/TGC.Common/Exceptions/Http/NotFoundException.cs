﻿namespace TGC.Common.Exceptions.Http;

[Serializable]
public class NotFoundException : Exception
{
	public NotFoundException() : base("Entity was not found.")
	{
	}

	public NotFoundException(string? message) : base(message)
	{
	}

	public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}