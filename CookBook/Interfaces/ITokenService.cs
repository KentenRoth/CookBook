using CookBook.Models;

namespace CookBook.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
    Task<string> CreateRefreshToken(AppUser user, string ipAddress);
    void SetRefreshTokenCookie(HttpResponse response, string refreshToken);
    Task ValidateRefreshToken(string refreshToken);
}