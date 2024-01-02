using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TGC.Core.Exceptions.Abstractions;
using TGC.Core.Serialization;

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

	public static IServiceCollection AddCoreSerialization(this IServiceCollection services)
	{
		services.AddTransient<IJsonSerializer, CoreJsonSerializer>();
		return services;
	}
}
