namespace TGC.Communication.cqrs;

public interface ICommandHandler
{
	public Task<IResult<ICommandResponse>> Handle<TCommand>(TCommand command) where TCommand : ICommand;
	public bool Accepts(ICommand command);
}