using CookBook.DTOs;

namespace CookBook.Helpers;

public class ServiceResponseHelper
{
    public static ServiceResponseDto<T> CreateSuccessResponse<T>(T data, string message = "")
    {
        return new ServiceResponseDto<T>
        {
            Data = data,
            Message = message,
            Success = true
        };
    }
    public static ServiceResponseDto<T> CreateErrorResponse<T>(string message)
    {
        return new ServiceResponseDto<T>
        {
            Data = default,
            Message = message,
            Success = false
        };
    }
}