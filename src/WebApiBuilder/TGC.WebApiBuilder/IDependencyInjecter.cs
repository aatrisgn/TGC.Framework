using Microsoft.Extensions.DependencyInjection;

namespace TGC.WebApiBuilder;
public interface IDependencyInjecter
{
	void InjectServices(IServiceCollection serviceCollection);
}
