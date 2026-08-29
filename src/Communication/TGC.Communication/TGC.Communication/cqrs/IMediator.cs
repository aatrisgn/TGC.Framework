namespace TGC.Communication.cqrs;

public interface IMediator
{
	public Task<IResult<TCommandResponse>> HandleCommandAsync<TCommand, TCommandResponse>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand where TCommandResponse :  ICommandResponse;
	public Task<IResult<TQueryResponse>> HandleQueryAsync<TQuery, TQueryResponse>(TQuery command, CancellationToken cancellationToken) where TQuery : IQuery where TQueryResponse : IQueryResponse;
}