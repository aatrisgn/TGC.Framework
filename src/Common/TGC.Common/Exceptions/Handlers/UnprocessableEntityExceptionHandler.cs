using System.Net;
using TGC.Common.Exceptions.Http;

namespace TGC.Common.Exceptions.Handlers;
internal class UnprocessableEntityExceptionHandler : BaseExceptionHandler
{
	public override Type ExceptionHandled => typeof(UnprocessableEntityException);

	public override int StatusCode => (int)HttpStatusCode.UnprocessableEntity;

	public override string Title => "Unprocessable Entity";
}
