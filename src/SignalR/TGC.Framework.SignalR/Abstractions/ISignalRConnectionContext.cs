namespace TGC.Framework.SignalR.Abstractions;
internal interface ISignalRConnectionContext
{
	void SetConnectionId(string connectionId);
	Task<string?> GetConnectionId();
}
