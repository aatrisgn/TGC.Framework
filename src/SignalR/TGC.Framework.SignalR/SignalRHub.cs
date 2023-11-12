using Microsoft.AspNetCore.SignalR;
using TGC.Framework.SignalR.Abstractions;

namespace TGC.Framework.SignalR;
internal class SignalRHub : Hub
{
	private readonly ISignalRConnectionContext _signalRConnectionContext;
	public SignalRHub(ISignalRConnectionContext signalRConnectionContext)
	{
		_signalRConnectionContext = signalRConnectionContext;
	}
	public override async Task OnConnectedAsync()
	{
		await Task.Run(() =>
		{
			_signalRConnectionContext.SetConnectionId(this.Context.ConnectionId);
		});
	}
}
