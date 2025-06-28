using System.Net;
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
        RegisterAccountRequestDto registerAccountRequestDto, HttpResponse response, string ipAddress)
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
            var refreshToken = await _tokenService.CreateRefreshToken(appUser, ipAddress);
            _tokenService.SetRefreshTokenCookie(response, refreshToken);

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
    
    public async Task<ServiceResponseDto<LoginAccountResponseDto>> Login(LoginAccountRequestDto loginAccountRequestDto, HttpResponse response, string ipAddress)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(loginAccountRequestDto.EmailOrUsername)
                       ?? await _userManager.FindByEmailAsync(loginAccountRequestDto.EmailOrUsername);
            if (user == null)
            {
                return ServiceResponseHelper.CreateErrorResponse<LoginAccountResponseDto>("Invalid username or password");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginAccountRequestDto.Password);
            if (!isPasswordValid)
            {
                return ServiceResponseHelper.CreateErrorResponse<LoginAccountResponseDto>("Invalid username or password");
            }

            var accessToken = await _tokenService.CreateToken(user);
            var refreshToken = await _tokenService.CreateRefreshToken(user, ipAddress);
            _tokenService.SetRefreshTokenCookie(response, refreshToken);

            var loginResponse = new LoginAccountResponseDto
            {
                AccessToken = accessToken
            };
            
            return ServiceResponseHelper.CreateSuccessResponse(loginResponse, "Login successful");
        }
        catch (Exception ex)
        {
            return ServiceResponseHelper.CreateErrorResponse<LoginAccountResponseDto>(ex.Message);
        }
    }
    
    public async Task<ServiceResponseDto<EmptyDto>> Logout(HttpRequest request, HttpResponse response, string ipAddress)
    {
        try
        {
            var refreshToken = request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return ServiceResponseHelper.CreateErrorResponse<EmptyDto>("No refresh token found");
            }

            await _tokenService.RevokeRefreshToken(refreshToken, ipAddress);
            response.Cookies.Delete("refreshToken");

            return ServiceResponseHelper.CreateSuccessResponse(new EmptyDto(), "Logout successful");
        }
        catch (Exception ex)
        {
            return ServiceResponseHelper.CreateErrorResponse<EmptyDto>(ex.Message);
        }
    }
    
    public async Task<ServiceResponseDto<EmptyDto>> LogoutAll(HttpRequest request, HttpResponse response, string ipAddress)
    {
        try
        {
            var refreshToken = request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return ServiceResponseHelper.CreateErrorResponse<EmptyDto>("No refresh token found");
            }
            var user = await _tokenService.GetUserFromRefreshToken(refreshToken);
            var userId = user.Id;
            await _tokenService.RevokeAllRefreshTokens(userId, ipAddress);
            response.Cookies.Delete("refreshToken");

            return ServiceResponseHelper.CreateSuccessResponse(new EmptyDto(), "Logged out from all devices successfully");
        }
        catch (Exception ex)
        {
            return ServiceResponseHelper.CreateErrorResponse<EmptyDto>(ex.Message);
        }
    }
}