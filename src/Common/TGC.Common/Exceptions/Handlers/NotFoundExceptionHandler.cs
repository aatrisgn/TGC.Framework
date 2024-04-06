using System.Net;
using TGC.Common.Exceptions.Http;

namespace TGC.Common.Exceptions.Handlers;
internal class NotFoundExceptionHandler : BaseExceptionHandler
{
	public override Type ExceptionHandled => typeof(NotFoundException);

	public override int StatusCode => (int)HttpStatusCode.NotFound;

	public override string Title => "Not Found";
}
