using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CookBook.Data;
using CookBook.Interfaces;
using CookBook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CookBook.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;
    private readonly ApplicationDBContext _context;
    private readonly UserManager<AppUser> _userManager;
    
    public TokenService(IConfiguration config, UserManager<AppUser> userManager, ApplicationDBContext context)
    {
        _config = config;
        _userManager = userManager;
        _context = context;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
    }

    public async Task<string> CreateToken(AppUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault();
        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
        };

        if (role != null)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
        var now = DateTime.UtcNow;
        var expires = now.AddMinutes(15);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            SigningCredentials = creds,
            NotBefore = now,
            Issuer = _config["JWT:Issuer"],
            Audience = _config["JWT:Audience"]
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    public async Task<string> CreateRefreshToken(AppUser user, string ipAddress)
    {
        var refreshToken = Guid.NewGuid().ToString();
        var now = DateTime.UtcNow;
        var expiryDate = DateTime.UtcNow.AddMonths(3);

        var refreshTokenEntity = new RefreshTokens
        {
            UserId = user.Id,
            Token = refreshToken,
            CreatedAt = now,
            CreatedByIp = ipAddress,
            ExpiresAt = expiryDate,
            IsRevoked = false,
            User = user
        };

        _context.RefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync();

        return refreshToken;
    }
    
    
}