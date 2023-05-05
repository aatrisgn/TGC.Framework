using System.Text.Json;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using TGC.HealthChecks;

namespace TGC.OpenAPISpecGeneration;

internal class OpenAPIAvailableHealthCheck : IHealthCheckExecutor
{
	private readonly IServer _server;
	private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

	public OpenAPIAvailableHealthCheck(IServer server, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
	{
		_server = server ?? throw new ArgumentNullException(nameof(server));
		_apiVersionDescriptionProvider = apiVersionDescriptionProvider ?? throw new ArgumentNullException(nameof(apiVersionDescriptionProvider));
	}

	public async Task<HealthCheckExecutionResult> CheckHealth()
	{
		if (_server.Features != null)
		{
			var addressFeature = _server.Features.Get<IServerAddressesFeature>();
			if (addressFeature != null)
			{
				var localhostAddress = addressFeature.Addresses;
				using (var client = new HttpClient())
				{
					var baseAddress = localhostAddress.FirstOrDefault();
					if (baseAddress != null)
					{
						client.BaseAddress = new Uri(baseAddress);

						foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
						{
							var response = await client.GetAsync($"/swagger/{description.GroupName}/swagger.json");

							if (response.IsSuccessStatusCode)
							{
								var content = await response.Content.ReadAsStringAsync();

								if (string.IsNullOrEmpty(content) == false)
								{
									try
									{
										JsonDocument.Parse(content);
									}
									catch (JsonException)
									{
										return HealthCheckExecutionResult.Unhealthy("OpenAPI definition is not valid JSON.");
									}
								}
							}
						}
						return HealthCheckExecutionResult.Healthy("OpenAPI definition is available.");
					}
				}
			}
		}
		return HealthCheckExecutionResult.Unhealthy("OpenAPI definition is not available.");
	}
}
