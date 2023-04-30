using Microsoft.AspNetCore.Builder;

namespace TGC.WebApiBuilder;
public interface IApplicationBuilderInstaller
{
	public void Install(IApplicationBuilder webApplication);
}
