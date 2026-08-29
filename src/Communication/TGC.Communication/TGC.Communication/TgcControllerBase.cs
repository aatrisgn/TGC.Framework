using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TGC.Communication;

[ApiController]
[Route("api")]
[Produces("application/json", [])]
[Consumes("application/json", [])]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public abstract class TgcControllerBase : ControllerBase
{
}
