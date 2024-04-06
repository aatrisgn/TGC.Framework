using System.Net;
using TGC.Common.Exceptions.Http;

namespace TGC.Common.Exceptions.Handlers;
internal class GoneExceptionHandler : BaseExceptionHandler
{
	public override Type ExceptionHandled => typeof(GoneException);

	public override int StatusCode => (int)HttpStatusCode.Gone;

	public override string Title => "Gone";
}
