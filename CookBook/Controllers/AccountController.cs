using CookBook.DTOs.Account.Request;
using CookBook.DTOs.Account.Response;
using CookBook.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers;


[Route("api/account")]

public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromServices] IValidator<RegisterAccountRequestDto> validator, [FromBody] RegisterAccountRequestDto registerDto)
    {
        var validationResult = await validator.ValidateAsync(registerDto);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(e => e.ErrorMessage).ToArray()
                );

            return BadRequest(new ValidationProblemDetails(errors));
        }

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var response = await _accountService.Register(registerDto, Response, ipAddress);

        if (!response.Success)
        {
            return StatusCode(500, response.Message);
        }

        return Ok(response.Data);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginAccountRequestDto loginDto)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var response = await _accountService.Login(loginDto, Response, ipAddress);
        
        if (!response.Success)
        {
            return BadRequest(response.Message);
        }
        
        return Ok(response.Data);
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var response = await _accountService.Logout(Request, Response, ipAddress);
        
        if (!response.Success)
        {
            return BadRequest(response.Message);
        }
        
        return Ok(response.Data);
    }
    
    [HttpPost("logoutall")]
    public async Task<IActionResult> LogoutAll()
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var response = await _accountService.LogoutAll(Request, Response, ipAddress);
        
        if (!response.Success)
        {
            return BadRequest(response.Message);
        }
        
        return Ok(response.Data);
    }
    
    [HttpGet("me")]
    public async Task<IActionResult> GetMe() 
    {
        var response = await _accountService.GetMe(Request);
        
        if (!response.Success)
        {
            return BadRequest(response.Message);
        }
        
        return Ok(response.Data);
    }
    
    [HttpPut("usersettings")]
    public async Task<IActionResult> UpdateUserSettings([FromBody] UpdateUserSettingsRequestDto updateUserSettingsRequestDto)
    {
        var response = await _accountService.UpdateUserSettings(updateUserSettingsRequestDto, Request);
        
        if (!response.Success)
        {
            return BadRequest(response.Message);
        }
        
        return Ok(response.Data);
    }

    [HttpPut("user")]
    public async Task<IActionResult> UpdateUser(
        [FromServices] IValidator<UpdateUserRequestDto> validator,
        [FromBody] UpdateUserRequestDto updateUserRequestDto)
    {
        var validationResult = await validator.ValidateAsync(updateUserRequestDto);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(e => e.ErrorMessage).ToArray()
                );

            return BadRequest(new ValidationProblemDetails(errors));
        }

        var response = await _accountService.UpdateUser(updateUserRequestDto, Request);
        if (!response.Success)
        {
            return BadRequest(response.Message);
        }

        return Ok(response.Data);
    }

    [HttpDelete("user")]
    public async Task<IActionResult> DeleteUser()
    {
        var response = await _accountService.DeleteUser(Request);
        if (!response.Success)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Data);
    }
}