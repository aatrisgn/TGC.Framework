namespace TGC.HealthChecks;
/// <summary>
/// Implement this interface and inject the implementation to register an additional health check
/// </summary>
public interface IHealthCheckExecutor
{
	/// <summary>
	/// Logic to check whether a given part of the application is healthy.
	/// </summary>
	/// <returns>A result indicating the status of the health check</returns>
	Task<HealthCheckExecutionResult> CheckHealth();
}
