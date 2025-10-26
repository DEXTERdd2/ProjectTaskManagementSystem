using Microsoft.AspNetCore.Mvc;
using GamePlanBackend.Application.Common.ResponseType;
using ProjectManagementSystem.Application.DTOs.UserDtos;
using ProjectManagementSystem.Domain.Interference;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var result = await _authService.RegisterAsync(request);
            return StatusCode(result.statusCode, result);
        }
        catch (Exception ex)
        {
            var errorResponse = new ResponseModel
            {
                statusCode = 500,
                ResponseMessage = "Registration failed: " + ex.Message,
                IsError = true,
                Success = false
            };
            return StatusCode(500, errorResponse);
        }
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await _authService.LoginAsync(request);
            return StatusCode(result.statusCode, result);
        }
        catch (Exception ex)
        {
            var errorResponse = new ResponseModel
            {
                statusCode = 500,
                ResponseMessage = "Login failed: " + ex.Message,
                IsError = true,
                Success = false
            };
            return StatusCode(500, errorResponse);
        }
    }
}
