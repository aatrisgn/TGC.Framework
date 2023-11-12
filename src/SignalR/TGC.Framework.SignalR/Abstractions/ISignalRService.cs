namespace TGC.Framework.SignalR.Abstractions;

/// <summary>
/// Represents an abstraction for SignalR to invoke sending information to subscribed clients.
/// </summary>
public interface ISignalRService
{
	/// <summary>
	/// Sends a message to all connected clients.
	/// </summary>
	/// <param name="method">The method name to invoke on the clients.</param>
	/// <param name="value">The data to send to the clients.</param>
	/// <returns>A task that represents the asynchronous send operation.</returns>
	Task SendAllAsync(string method, object value);

	/// <summary>
	/// Sends a message to the calling client.
	/// </summary>
	/// <param name="method">The method name to invoke on the calling client.</param>
	/// <param name="value">The data to send to the calling client.</param>
	/// <returns>A task that represents the asynchronous send operation.</returns>
	Task SendCallerAsync(string method, object value);

	/// <summary>
	/// Sends a message to all connected clients except the calling client.
	/// </summary>
	/// <param name="method">The method name to invoke on the other clients.</param>
	/// <param name="value">The data to send to the other clients.</param>
	/// <returns>A task that represents the asynchronous send operation.</returns>
	Task SendOthersAsync(string method, object value);
}
