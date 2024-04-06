using System.Net;
using TGC.Common.Exceptions.Http;

namespace TGC.Common.Exceptions.Handlers;
internal class ConflictExceptionHandler : BaseExceptionHandler
{
	public override Type ExceptionHandled => typeof(ConflictException);

	public override int StatusCode => (int)HttpStatusCode.Conflict;

	public override string Title => "Conflict";
}
