using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using TGC.Common.Serialization;

namespace TGC.Common.Exceptions.Extensions;
public static class IServiceCollectionExtensions
{
	public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
	{
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		var exceptionDescriptorType = typeof(IExceptionDescriptor);

		// Find types implementing the interface and are not interfaces or abstract classes
		List<Type> implementingTypes = new List<Type>();

		foreach (var assembly in assemblies)
		{
			foreach (var type in assembly.GetTypes())
			{
				if (exceptionDescriptorType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
				{
					implementingTypes.Add(type);
				}
			}
		}

		foreach (var implementation in implementingTypes)
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
