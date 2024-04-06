using System.Diagnostics.CodeAnalysis;

namespace TGC.Common.Exceptions;
/// <summary>
/// Provides methods to handle HTTP exceptions.
/// </summary>
public static class HttpExceptionFactory
{
	/// <summary>
	/// Throws an exception of type <typeparamref name="T"/> if the specified value is null.
	/// </summary>
	/// <typeparam name="T">The type of exception to throw.</typeparam>
	/// <param name="value">The value to check for null.</param>
	/// <param name="exceptionMessage">The message to include in the exception if <paramref name="value"/> is null.</param>
	/// <exception cref="InvalidOperationException">Thrown if <paramref name="value"/> is null.</exception>
	/// <exception cref="InvalidCastException">Thrown if an instance of <typeparamref name="T"/> cannot be instantiated.</exception>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="exceptionMessage"/> is null.</exception>
	/// <remarks>
	/// If <paramref name="value"/> is null, an exception of type <typeparamref name="T"/> is thrown.
	/// If <paramref name="exceptionMessage"/> is null, <see cref="ArgumentNullException"/> is thrown.
	/// </remarks>
	public static void ThrowIfNull<T>([NotNull] object? value, string exceptionMessage) where T : Exception
	{
		if (value == null)
		{
			if (exceptionMessage != null)
			{
				var exception = Activator.CreateInstance(typeof(T), exceptionMessage) as T;
				if (exception != null)
				{
					throw exception;
				}
				else
				{
					throw new InvalidCastException("Could not instantiate instance of T");
				}
			}
			else
			{
				throw new InvalidOperationException(exceptionMessage);
			}
		}
		throw new InvalidOperationException(exceptionMessage);
	}
}
