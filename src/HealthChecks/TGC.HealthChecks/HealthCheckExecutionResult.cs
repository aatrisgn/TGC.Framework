namespace TGC.HealthChecks;
public class HealthCheckExecutionResult
{
	public string HealthMessage { get; }
	public HealthCheckExecutionStatusEnum HealthStatus { get; }
	public string HealthStatusName { get; }
	public static HealthCheckExecutionResult Healthy(string message)
	{
		return new HealthCheckExecutionResult(message, HealthCheckExecutionStatusEnum.Healthy);
	}

	public static HealthCheckExecutionResult Warning(string message)
	{
		return new HealthCheckExecutionResult(message, HealthCheckExecutionStatusEnum.Warning);
	}

	public static HealthCheckExecutionResult Unhealthy(string message)
	{
		return new HealthCheckExecutionResult(message, HealthCheckExecutionStatusEnum.Unhealthy);
	}

	private HealthCheckExecutionResult(string healthMessage, HealthCheckExecutionStatusEnum status)
	{
		var statusName = Enum.GetName(status);

		if (statusName != null)
		{
			HealthMessage = healthMessage;
			HealthStatus = status;
			HealthStatusName = statusName;
		}
		else
		{
			throw new ArgumentNullException("Not possible to GetName of status enum.");
		}
	}
}
