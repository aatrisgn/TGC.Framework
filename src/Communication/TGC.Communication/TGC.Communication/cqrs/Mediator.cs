namespace TGC.Communication.cqrs;

public class Mediator : IMediator
{
	private readonly IEnumerable<IQueryHandler> _queryHandlers;
	private readonly IEnumerable<ICommandHandler> _commandHandlers;

	public Mediator(IEnumerable<IQueryHandler> queryHandlers, IEnumerable<ICommandHandler> commandHandlers)
	{
		_commandHandlers = commandHandlers;
		_queryHandlers = queryHandlers;
	}


	public async Task<IResult<TCommandResponse>> HandleCommandAsync<TCommand, TCommandResponse>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand where TCommandResponse : ICommandResponse
	{
		var handler = _commandHandlers.Single(h => h.Accepts(command));
		var result = await handler.Handle<TCommand>(command);
		return result as IResult<TCommandResponse> ?? Result<TCommandResponse>.AsInternalServerError("Could not handle command."); //Should log error with more details.
	}

	public async Task<IResult<TQueryResponse>> HandleQueryAsync<TQuery, TQueryResponse>(TQuery command, CancellationToken cancellationToken) where TQuery : IQuery where TQueryResponse : IQueryResponse
	{
		var handler = _queryHandlers.Single(h => h.Accepts(command));
		var result = await handler.Handle<TQuery>(command);
		return result as IResult<TQueryResponse> ?? Result<TQueryResponse>.AsInternalServerError("Could not handle query."); //Should log error with more details.
	}

	// Right now the code above could most likely be more DRY. But it's good enough for now.
	// private async Task<TResponse> HandleCommandQuery<TRequest, TResponse>(TRequest request)
	// {
	// 	ArgumentNullException.ThrowIfNull(request);
	//
	// 	if (request is ICommand)
	// 	{
	//
	// 	}
	// 	else if (request is IQuery)
	// 	{
	// 		var handler = _queryHandlers.Single(h => h.Accepts(request as IQuery));
	// 		var result = await handler.Handle<TRequest, TResponse>(request);
	// 		return result;
	// 	}
	// 	throw new Exception("No handler found for request");
	// }
}
