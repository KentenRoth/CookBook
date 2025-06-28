using CookBook.DTOs.Account.Request;
using CookBook.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CookBook.Controllers;


[Route("api/account")]
[ApiController]

public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterAccountRequestDto registerDto)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        
        var response = await _accountService.Register(registerDto, Response, ipAddress);
        
        if (!response.Success)
        {
            return StatusCode(500, response.Message);  
        }

        return Ok(response.Data);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginAccountRequestDto loginDto)
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
}