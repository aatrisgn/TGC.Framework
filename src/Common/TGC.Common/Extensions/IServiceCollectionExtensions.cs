using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using TGC.Common.Serialization;

namespace TGC.Common.Exceptions.Extensions;
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

	public static IServiceCollection AddCoreSerialization(this IServiceCollection services, Action<JsonSerializerOptions> jsonSerializerOptions)
	{
		var serializationOptions = new JsonSerializerOptions();
		jsonSerializerOptions.Invoke(serializationOptions);

		services.AddCoreSerialization(serializationOptions);
		return services;
	}

	public static IServiceCollection AddCoreSerialization(this IServiceCollection services, JsonSerializerOptions jsonSerializerOptions)
	{
		services.AddSingleton<IJsonSerializer>(new CoreJsonSerializer(jsonSerializerOptions));
		return services;
	}
}
