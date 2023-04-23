namespace TGC.Configuration;
public interface IAppSettings
{
	string GetString(string key);
	int GetInt(string key);
	bool GetBoolen(string key);
	double GetDouble(string key);
	T GetTyped<T>();
	T GetTyped<T>(string key);
}
