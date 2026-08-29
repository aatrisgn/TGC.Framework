using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace TGC.Communication.cqrs;

public interface IResult<out T> where T : IMediatorResponse
{
	bool IsSuccess { get; }
	string? Error { get; }
	HttpStatusCode? StatusCode { get; }
	T? Value { get; }
	public ActionResult ToActionResult();
}


public class Result<T> : IResult<T> where T : IMediatorResponse
{
	public bool IsSuccess { get; }
	public string? Error { get; }
	public HttpStatusCode? StatusCode { get; }
	public T? Value { get; }

	private Result(bool isSuccess, T? value, HttpStatusCode statusCode, string? error)
	{
		IsSuccess = isSuccess;
		StatusCode = statusCode;
		Value = value;
		Error = error;
	}

	public ActionResult ToActionResult()
	{
		return StatusCode switch
		{
			HttpStatusCode.OK => new OkObjectResult(Value),
			HttpStatusCode.BadRequest => new BadRequestObjectResult(Error),
			HttpStatusCode.NotFound => new NotFoundObjectResult(Error),
			HttpStatusCode.Conflict => new ConflictObjectResult(Error),
			_ => throw new ArgumentOutOfRangeException()
		};
	}
	
	public static Result<T> AsOk(T value) => new Result<T>(true, value, HttpStatusCode.OK, null);
	public static Result<T> AsBadRequest(string error) => new Result<T>(false, default, HttpStatusCode.BadRequest, error);
	public static Result<T> AsNotFound(string error) => new Result<T>(false, default, HttpStatusCode.NotFound, error);
	public static Result<T> AsConflict(string error) => new Result<T>(false, default, HttpStatusCode.Conflict, error);
	//TODO: We need to find a way to handle this
	public static Result<T> AsInternalServerError(string error) => new Result<T>(false, default, HttpStatusCode.InternalServerError, error);
}