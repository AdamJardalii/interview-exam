using Microsoft.AspNetCore.Mvc;
using CadabraTest.API.Models;
using CadabraTest.API.Services;

namespace CadabraTest.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var user = await _authService.RegisterAsync(dto.Email, dto.Password);
        return Ok(new { user.Id, user.Email });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto.Email, dto.Password);
        if (token == null) return Unauthorized(new { error = "Invalid credentials" });
        return Ok(new { token });
    }
}


public record RegisterDto(string Email, string Password);
public record LoginDto(string Email, string Password);
