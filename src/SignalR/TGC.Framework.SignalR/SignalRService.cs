using Microsoft.AspNetCore.SignalR;
using TGC.Framework.SignalR.Abstractions;

namespace TGC.Framework.SignalR;
internal class SignalRService : ISignalRService
{
	private readonly IHubContext<SignalRHub> _hub;
	private readonly ISignalRConnectionContext _signalRConnectionContext;

	public SignalRService(IHubContext<SignalRHub> hub, ISignalRConnectionContext signalRConnectionContext)
	{
		_hub = hub;
		_signalRConnectionContext = signalRConnectionContext;
	}

	public async Task SendAllAsync(string method, object value)
	{
		await _hub.Clients.All.SendAsync(method, value);
	}
	public async Task SendCallerAsync(string method, object value)
	{
		var connectionId = await _signalRConnectionContext.GetConnectionId();
		if (connectionId != null)
		{
			await _hub.Clients.Client(connectionId).SendAsync(method, value);
		}
	}
	public async Task SendOthersAsync(string method, object value)
	{
		var connectionId = await _signalRConnectionContext.GetConnectionId();
		if (connectionId != null)
		{
			await _hub.Clients.AllExcept(connectionId).SendAsync(method, value);
		}
		await _hub.Clients.All.SendAsync(method, value);
	}
}
