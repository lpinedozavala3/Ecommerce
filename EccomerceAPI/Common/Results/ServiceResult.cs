using System;
using System.Net;

namespace EccomerceAPI.Common.Results;

public class ServiceResult
{
    public bool IsSuccess { get; }
    public string Message { get; }
    public string[] Errors { get; }
    public HttpStatusCode StatusCode { get; }

    protected ServiceResult(bool isSuccess, HttpStatusCode statusCode, string message, string[]? errors)
    {
        IsSuccess = isSuccess;
        StatusCode = statusCode;
        Message = message;
        Errors = errors ?? Array.Empty<string>();
    }

    public static ServiceResult Success(string message, HttpStatusCode statusCode = HttpStatusCode.OK) =>
        new(true, statusCode, message, Array.Empty<string>());

    public static ServiceResult Failure(
        string message,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest,
        params string[] errors) =>
        new(false, statusCode, message, errors is { Length: > 0 } ? errors : new[] { message });
}

public sealed class ServiceResult<T> : ServiceResult
{
    public T? Data { get; }

    private ServiceResult(bool isSuccess, HttpStatusCode statusCode, T? data, string message, string[]? errors)
        : base(isSuccess, statusCode, message, errors)
    {
        Data = data;
    }

    public static ServiceResult<T> Success(
        T data,
        HttpStatusCode statusCode = HttpStatusCode.OK,
        string message = "") =>
        new(true, statusCode, data, message, Array.Empty<string>());

    public static ServiceResult<T> Failure(
        string message,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest,
        params string[] errors) =>
        new(false, statusCode, default, message, errors is { Length: > 0 } ? errors : new[] { message });
}
