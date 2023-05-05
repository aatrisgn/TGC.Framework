using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using TGC.WebApiBuilder;

namespace TGC.OpenAPISpecGeneration;

internal class OpenAPIInstaller : IApplicationBuilderInstaller
{
	private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;
	public OpenAPIInstaller(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
	{
		_apiVersionDescriptionProvider = apiVersionDescriptionProvider;
	}

	public void Install(IApplicationBuilder webApplication)
	{
		webApplication.UseSwagger();
		webApplication.UseSwaggerUI(options =>
		{
			foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
			{
				options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
					description.GroupName.ToUpperInvariant());
			}
		});
	}
}
