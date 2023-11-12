using TGC.Framework.SignalR.Abstractions;

namespace TGC.Framework.SignalR;
internal class SignalRConnectionContext : ISignalRConnectionContext
{
	private string? connectionId;
	public Task<string?> GetConnectionId()
	{
		return Task.FromResult(connectionId);
	}

	public void SetConnectionId(string connectionId)
	{
		this.connectionId = connectionId;
	}
}
