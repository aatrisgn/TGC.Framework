using Microsoft.AspNetCore.Routing;

namespace TGC.WebApiBuilder;
public interface IEndpointRouteBuilderInstaller
{
	public void Install(IEndpointRouteBuilder webApplication);
}
