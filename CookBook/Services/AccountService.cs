using CookBook.Data;
using CookBook.DTOs;
using CookBook.DTOs.Account.Request;
using CookBook.DTOs.Account.Response;
using CookBook.Helpers;
using CookBook.Interfaces;
using CookBook.Models;
using Microsoft.AspNetCore.Identity;

namespace CookBook.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDBContext _context;
    private readonly ITokenService _tokenService;
    
    public AccountService(UserManager<AppUser> userManager, ApplicationDBContext context, ITokenService tokenService)
    {
        _userManager = userManager;
        _context = context;
        _tokenService = tokenService;
    }
    
    public async Task<ServiceResponseDto<RegisterAccountResponseDto>> Register(
        RegisterAccountRequestDto registerAccountRequestDto, HttpResponse response)
    {
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var appUser = new AppUser()
            {
                UserName = registerAccountRequestDto.Username,
                Email = registerAccountRequestDto.Email,
                Name = registerAccountRequestDto.Name,
            };
            
            var createUser = await _userManager.CreateAsync(appUser, registerAccountRequestDto.Password);
            
            if (!createUser.Succeeded)
            {
                var errors = string.Join(", ", createUser.Errors.Select(e => e.Description));
                return ServiceResponseHelper.CreateErrorResponse<RegisterAccountResponseDto>(errors);
            }
            
            var userSettings = new UserSettings
            {
                ColorMode = "light",
                UserId = appUser.Id,
                User = appUser
            };
            
            _context.UserSettings.Add(userSettings);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            var roleResult = await _userManager.AddToRoleAsync(appUser, "Pending");
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return ServiceResponseHelper.CreateErrorResponse<RegisterAccountResponseDto>($"Role assignment failed: {errors}");
            }
            
            var accessToken = await _tokenService.CreateToken(appUser);

            var registerAccountResponse = new RegisterAccountResponseDto
            {
                AccessToken = accessToken
            };
            
            return ServiceResponseHelper.CreateSuccessResponse(registerAccountResponse, "Registration successful");

            
        } catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return ServiceResponseHelper.CreateErrorResponse<RegisterAccountResponseDto>(ex.Message);
        }
    }
}