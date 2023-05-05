using FluentAssertions;

namespace TGC.HealthChecks.Tests;

internal class HealthCheckExecutionResultTests
{
	[Test]
	public async Task WHEN_CreatingHealthyResult_THEN_ResultStatusAndMessageIsHealthy()
	{
		var healthCheck = HealthCheckExecutionResult.Healthy("Test message");

		healthCheck.HealthStatus.Should().Be(HealthCheckExecutionStatusEnum.Healthy);
		healthCheck.HealthStatusName.Should().Be(Enum.GetName(HealthCheckExecutionStatusEnum.Healthy));
	}

	[Test]
	public async Task WHEN_CreatingUnhealthyResult_THEN_ResultStatusAndMessageIsUnhealthy()
	{
		var healthCheck = HealthCheckExecutionResult.Unhealthy("Test message");

		healthCheck.HealthStatus.Should().Be(HealthCheckExecutionStatusEnum.Unhealthy);
		healthCheck.HealthStatusName.Should().Be(Enum.GetName(HealthCheckExecutionStatusEnum.Unhealthy));
	}

	[Test]
	public async Task WHEN_CreatingWarningResult_THEN_ResultStatusAndMessageIsWarning()
	{
		var healthCheck = HealthCheckExecutionResult.Warning("Test message");

		healthCheck.HealthStatus.Should().Be(HealthCheckExecutionStatusEnum.Warning);
		healthCheck.HealthStatusName.Should().Be(Enum.GetName(HealthCheckExecutionStatusEnum.Warning));
	}

}
