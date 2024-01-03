namespace TGC.Common.Serialization;

public interface IJsonSerializer
{
	T? Deserialize<T>(string content);
	Task<T?> DeserializeAsync<T>(Stream content);
}
