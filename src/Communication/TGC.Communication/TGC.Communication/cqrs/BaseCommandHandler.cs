namespace TGC.Communication.cqrs;

public abstract class BaseCommandHandler<TCommand, TCommandResponse> where TCommand : class, ICommand where TCommandResponse :  ICommandResponse
{
	public virtual bool Accepts(ICommand query) => query is TCommand;
	protected TCommand GetTypedCommand(ICommand command) => command as TCommand ?? throw new InvalidOperationException($"Could not cast command {command.GetType().FullName} as {typeof(TCommand).FullName}");

	protected Result<TCommandResponse> AsOk(TCommandResponse response)
	{
		return Result<TCommandResponse>.AsOk(response);
	}
	
	protected Result<TCommandResponse> AsConflict(string error)
	{
		return Result<TCommandResponse>.AsConflict(error);
	}
	
	protected Result<TCommandResponse> AsBadRequest(string error)
	{
		return Result<TCommandResponse>.AsBadRequest(error);
	}
	
	protected Result<TCommandResponse> AsNotFound(string error)
	{
		return Result<TCommandResponse>.AsNotFound(error);
	}
}