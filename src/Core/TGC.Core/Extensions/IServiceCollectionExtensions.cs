using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TGC.Core.Exceptions.Abstractions;

namespace TGC.Core.Exceptions.Extensions;
public static class IServiceCollectionExtensions
{
	public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
	{
		var assembly = Assembly.GetEntryAssembly();
		var exceptionDescriptorType = typeof(IExceptionDescriptor);

		var exceptionDescriptorImplementations = assembly!.GetTypes()
			.Where(t => exceptionDescriptorType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
			.ToList();

		foreach (var implementation in exceptionDescriptorImplementations)
		{
			services.AddScoped(exceptionDescriptorType, implementation);
		}

		services.AddExceptionHandler<ExceptionHandler>();
		return services;
	}
}
