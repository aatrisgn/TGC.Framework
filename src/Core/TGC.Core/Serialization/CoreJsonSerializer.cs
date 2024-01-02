using System.Text.Json;

namespace TGC.Core.Serialization;

public class CoreJsonSerializer : IJsonSerializer
{
	public T? Deserialize<T>(string content)
	{
		return JsonSerializer.Deserialize<T>(content);
	}

	public async Task<T?> DeserializeAsync<T>(Stream content)
	{
		ArgumentNullException.ThrowIfNull(content);
		return await JsonSerializer.DeserializeAsync<T>(content);
	}
}
