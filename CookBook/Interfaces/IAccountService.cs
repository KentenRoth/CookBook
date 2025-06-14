using CookBook.DTOs;
using CookBook.DTOs.Account.Request;
using CookBook.DTOs.Account.Response;

namespace CookBook.Interfaces;

public interface IAccountService
{
    Task<ServiceResponseDto<RegisterAccountResponseDto>> Register(RegisterAccountRequestDto registerDto,
        HttpResponse response);
}