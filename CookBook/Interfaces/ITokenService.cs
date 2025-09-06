using CookBook.DTOs.Account.Response;
using CookBook.Models;

namespace CookBook.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
    Task<string> CreateRefreshToken(AppUser user, string ipAddress);
    void SetRefreshTokenCookie(HttpResponse response, string refreshToken);
    Task ValidateRefreshToken(string refreshToken, string userId);
    Task RevokeRefreshToken(string refreshToken, string ipAddress);
    Task RevokeAllRefreshTokens(string userId, string ipAddress);
    Task<AppUser> GetUserFromRefreshToken(string refreshToken);
    Task<RefreshDto> RefreshTokens(string refreshToken, HttpResponse response, string ipAddress);
}