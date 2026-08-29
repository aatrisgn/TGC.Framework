namespace TGC.Communication.cqrs;

public interface IQueryHandler
{
	public Task<IResult<IQueryResponse>> Handle<TQuery>(TQuery query) where TQuery : IQuery;
	public bool Accepts(IQuery query);
}