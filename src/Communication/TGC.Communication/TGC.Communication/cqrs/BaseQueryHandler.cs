namespace TGC.Communication.cqrs;

public abstract class BaseQueryHandler<TQuery, TQueryResponse> where TQuery : class, IQuery where TQueryResponse :  IQueryResponse
{
	public virtual bool Accepts(IQuery query) => query is TQuery;
	protected TQuery GetTypedQuery(IQuery query) => query as TQuery;
	
	protected Result<TQueryResponse> AsOk(TQueryResponse response)
	{
		return Result<TQueryResponse>.AsOk(response);
	}
	
	protected Result<TQueryResponse> AsNotFound(string error)
	{
		return Result<TQueryResponse>.AsNotFound(error);
	}
	
	protected Result<TQueryResponse> AsNoContent()
	{
		return Result<TQueryResponse>.AsNoContent();
	}
}