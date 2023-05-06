using Microsoft.AspNetCore.Mvc.Versioning;
using TGC.HealthChecks.IoC;
using TGC.OpenAPISpecGeneration.IoC;
using TGC.WebApiBuilder;

namespace WebApplication1.Test
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddApiVersioning(o =>
			{
				o.AssumeDefaultVersionWhenUnspecified = true;
				o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
				o.ReportApiVersions = true;
				//o.ApiVersionReader = ApiVersionReader.Combine(
				//	new QueryStringApiVersionReader("api-version"),
				//	new HeaderApiVersionReader("X-Version"),
				//	new MediaTypeApiVersionReader("ver"));
			});

			builder.Services.AddVersionedApiExplorer(
			options =>
			{
				options.GroupNameFormat = "'v'VVV";
				options.SubstituteApiVersionInUrl = false;
			});

			var app = builder.BuildWebAPI();

			app.UseAuthorization();

			app.Run();
		}
	}
}