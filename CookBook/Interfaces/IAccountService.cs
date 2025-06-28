using CookBook.DTOs;
using CookBook.DTOs.Account.Request;
using CookBook.DTOs.Account.Response;

namespace CookBook.Interfaces;

public interface IAccountService
{
    Task<ServiceResponseDto<RegisterAccountResponseDto>> Register(RegisterAccountRequestDto registerDto,
        HttpResponse response, string ipAddress);
    Task<ServiceResponseDto<LoginAccountResponseDto>> Login(LoginAccountRequestDto loginDto, HttpResponse response,
        string ipAddress);
    Task<ServiceResponseDto<EmptyDto>> Logout(HttpRequest request, HttpResponse response, string ipAddress);
}