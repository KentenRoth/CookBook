using CookBook.Models;

namespace CookBook.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
}