using System.Net;
using TGC.Common.Exceptions.Http;

namespace TGC.Common.Exceptions.Handlers;
internal class ForbiddenExceptionHandler : BaseExceptionHandler
{
	public override Type ExceptionHandled => typeof(ForbiddenException);

	public override int StatusCode => (int)HttpStatusCode.Forbidden;

	public override string Title => "Forbidden";
}
