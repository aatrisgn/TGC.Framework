using System.Text.Json;

namespace TGC.Common.Serialization;

internal class CoreJsonSerializer : IJsonSerializer
{
	private JsonSerializerOptions _options;
	public CoreJsonSerializer(JsonSerializerOptions options)
	{
		_options = options;
	}

	public CoreJsonSerializer()
	{
		_options = new JsonSerializerOptions();
	}

	public T? Deserialize<T>(string content)
	{
		return JsonSerializer.Deserialize<T>(content, _options);
	}

	public async Task<T?> DeserializeAsync<T>(Stream content)
	{
		ArgumentNullException.ThrowIfNull(content);
		return await JsonSerializer.DeserializeAsync<T>(content);
	}
}
