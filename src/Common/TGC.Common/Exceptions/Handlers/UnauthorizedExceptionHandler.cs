using System.Net;
using TGC.Common.Exceptions.Http;

namespace TGC.Common.Exceptions.Handlers;
internal class UnauthorizedExceptionHandler : BaseExceptionHandler
{
	public override Type ExceptionHandled => typeof(UnauthorizedException);

	public override int StatusCode => (int)HttpStatusCode.Unauthorized;

	public override string Title => "Unauthorized";
}
