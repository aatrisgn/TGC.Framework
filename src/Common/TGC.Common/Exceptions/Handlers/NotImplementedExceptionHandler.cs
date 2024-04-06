using System.Net;
using NotImplementedException = TGC.Common.Exceptions.Http.NotImplementedException;

namespace TGC.Common.Exceptions.Handlers;
internal class NotImplementedExceptionHandler : BaseExceptionHandler
{
	public override Type ExceptionHandled => typeof(NotImplementedException);

	public override int StatusCode => (int)HttpStatusCode.NotImplemented;

	public override string Title => "Not Implemented";
}
