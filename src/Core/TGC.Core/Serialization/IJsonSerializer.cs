﻿namespace TGC.Core.Serialization;

internal interface IJsonSerializer
{
	T? Deserialize<T>(string content);
	Task<T?> DeserializeAsync<T>(Stream content);
}
