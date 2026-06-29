using Microsoft.AspNetCore.Mvc;

namespace TaskFlow.Api.Common;

public class ServiceResult<T>
{
    public bool Success { get; private set; }

    public T? Data { get; private set; }

    public ProblemDetails? Error { get; private set; }

    private ServiceResult()
    {
    }

    public static ServiceResult<T> Ok(T data)
    {
        return new ServiceResult<T>
        {
            Success = true,
            Data = data
        };
    }

    public static ServiceResult<T> Fail(
        int statusCode,
        string title,
        string detail)
    {
        return new ServiceResult<T>
        {
            Success = false,
            Error = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail
            }
        };
    }

    public static ServiceResult<T> Fail(ProblemDetails problemDetails)
    {
        return new ServiceResult<T>
        {
            Success = false,
            Error = problemDetails
        };
    }
}
