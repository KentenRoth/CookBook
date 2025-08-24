using System.Net;
using CookBook.Data;
using CookBook.DTOs;
using CookBook.DTOs.Account.Request;
using CookBook.DTOs.Account.Response;
using CookBook.Helpers;
using CookBook.Interfaces;
using CookBook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
    
    public async Task<ServiceResponseDto<MeAccountResponseDto>> GetMe(string userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return ServiceResponseHelper.CreateErrorResponse<MeAccountResponseDto>("User not found.");
        }
        
        var userSettings = await _context.UserSettings
            .FirstOrDefaultAsync(us => us.UserId == user.Id);

        var meDto = new MeAccountResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Username = user.UserName,
            CreatedAt = user.CreatedAt,
            UserSettings = new UserSettingsResponseDto
            {
                ColorMode = userSettings.ColorMode
            }
        };
        
        return ServiceResponseHelper.CreateSuccessResponse(meDto, "User retrieved successfully");
    }
    
    public async Task<ServiceResponseDto<EmptyDto>> UpdateUserSettings(UpdateUserSettingsRequestDto updateUserSettingsRequestDto, HttpRequest request)
    {
        var refreshToken = request.Cookies["refreshToken"];
        
        if (string.IsNullOrEmpty(refreshToken))
        {
            return ServiceResponseHelper.CreateErrorResponse<EmptyDto>("No refresh token found.");
        }

        var user = await _tokenService.GetUserFromRefreshToken(refreshToken);

        if (user == null)
        {
            return ServiceResponseHelper.CreateErrorResponse<EmptyDto>("Invalid refresh token.");
        }

        var userSettings = await _context.UserSettings
            .FirstOrDefaultAsync(us => us.UserId == user.Id);

        if (userSettings == null)
        {
            return ServiceResponseHelper.CreateErrorResponse<EmptyDto>("User settings not found.");
        }

        userSettings.ColorMode = updateUserSettingsRequestDto.ColorMode;
        
        _context.UserSettings.Update(userSettings);
        await _context.SaveChangesAsync();

        return ServiceResponseHelper.CreateSuccessResponse(new EmptyDto(), "User settings updated successfully");
    }
    
    public async Task<ServiceResponseDto<UpdateUserResponseDto>> UpdateUser(UpdateUserRequestDto dto,
        HttpRequest request)
    {
        var refreshToken = request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
        {
            return ServiceResponseHelper.CreateErrorResponse<UpdateUserResponseDto>("No refresh token found.");
        }
        var user = await _tokenService.GetUserFromRefreshToken(refreshToken);

        if (user == null)
        {
            return ServiceResponseHelper.CreateErrorResponse<UpdateUserResponseDto>("Invalid refresh token.");
        }
        
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.CurrentPassword);
        if (!isPasswordValid)
        {
            return ServiceResponseHelper.CreateErrorResponse<UpdateUserResponseDto>("Invalid password.");
        }
        
        var updateErrors = new List<string>();
        bool hasChanges = false;

        if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name != user.Name)
        {
            user.Name = dto.Name;
            hasChanges = true;
        }

        if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != user.Email)
        {
            user.Email = dto.Email;
            hasChanges = true;
        }

        if (!string.IsNullOrWhiteSpace(dto.Username) && dto.Username != user.UserName)
        {
            user.UserName = dto.Username;
            hasChanges = true;
        }

        if (!string.IsNullOrWhiteSpace(dto.NewPassword))
        {
            var passwordResult = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!passwordResult.Succeeded)
                updateErrors.AddRange(passwordResult.Errors.Select(e => e.Description));
        }

        if (hasChanges)
        {
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                updateErrors.AddRange(updateResult.Errors.Select(e => e.Description));
        }

        if (updateErrors.Any())
            return ServiceResponseHelper.CreateErrorResponse<UpdateUserResponseDto>(
                $"Update failed: {string.Join(", ", updateErrors)}"
            );

        var response = new UpdateUserResponseDto
        {
            Name = user.Name,
            Email = user.Email,
            Username = user.UserName
        };

        return ServiceResponseHelper.CreateSuccessResponse(response, "User updated successfully.");
    }

    public async Task<ServiceResponseDto<EmptyDto>> DeleteUser(HttpRequest request)
    {
        var refreshToken = request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
        {
            return ServiceResponseHelper.CreateErrorResponse<EmptyDto>("No refresh token found.");
        }

        var user = await _tokenService.GetUserFromRefreshToken(refreshToken);
        
        if (user == null)
        {
            return ServiceResponseHelper.CreateErrorResponse<EmptyDto>("Invalid refresh token.");
        }
        
        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return ServiceResponseHelper.CreateErrorResponse<EmptyDto>($"Delete failed: {string.Join(", ", errors)}");
        }
        
        return ServiceResponseHelper.CreateSuccessResponse(new EmptyDto(), "User deleted successfully");
        
    }
    
    
}