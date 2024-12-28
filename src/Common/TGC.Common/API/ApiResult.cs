using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace TGC.Common.API;

public class ApiResult
{
	public ApiBaseResult? Data { get; init; }
	private HttpStatusCode Status { get; init; }

	/// <summary>
	/// sadasd
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	public static ApiResult Ok(ApiBaseResult data)
	{
		return new ApiResult
		{
			Data = data,
			Status = HttpStatusCode.OK
		};
	}

	public static ApiResult Created(ApiBaseResult data)
	{
		return new ApiResult
		{
			Data = data,
			Status = HttpStatusCode.Created
		};
	}

	public static ApiResult NewResult(HttpStatusCode status)
	{
		return new ApiResult
		{
			Status = status
		};
	}

	public IActionResult ToActionResult()
	{
		switch (Status)
		{
			case HttpStatusCode.OK:
				return new OkObjectResult(Data);
			case HttpStatusCode.Created:
				if (Data is null)
				{
					throw new ArgumentNullException(nameof(Data));
				}
				return new CreatedResult(new Uri($"/{Data.Id}", UriKind.Relative), Data);
			default:
				return new ObjectResult(Data) { StatusCode = (int)Status };
		}
	}
}
