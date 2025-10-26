using GamePlanBackend.Application.Common.MiddleWare;
using GamePlanBackend.Application.Common.ResponseType;
using GamePlanBackend.Infrastructure.Auth.JWT;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectManagementSystem.Application.DTOs.UserDtos;
using ProjectManagementSystem.Domain.Interference;
using ProjectManagementSystem.Infrastructure.Data;


public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<ResponseModel> RegisterAsync(RegisterRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return ResponseData.NotSuccessResponse("Email and Password are required.");

            var email = request.Email.Trim().ToLowerInvariant();

            if (await _context.Tb_Users.AnyAsync(u => u.Email == email))
                return ResponseData.NotSuccessResponse("User already exists with this email.");

            if (!string.IsNullOrWhiteSpace(request.Username) &&
                await _context.Tb_Users.AnyAsync(u => u.UserName == request.Username))
                return ResponseData.NotSuccessResponse("Username already exists.");

            var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new Tb_User
            {
                Email = email,
                UserName = request.Username,
                PasswordHash = hash,
                CreatedAt = DateTime.UtcNow,
                Role = "User"
            };

            _context.Tb_Users.Add(user);
            await _context.SaveChangesAsync();

            return new ResponseModel
            {
                statusCode = 200,
                ResponseMessage = "User registered successfully.",
                DataModel = new { userId = user.Id }
            };
        }
        catch (Exception ex)
        {
            return ResponseData.ErrorResponse("Registration failed", ex.Message);
        }
    }

    public async Task<ResponseModel> LoginAsync(LoginRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return ResponseData.NotSuccessResponse("Email and Password are required.");

            var user = await _context.Tb_Users
       .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());



            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return new ResponseModel
                {
                    statusCode = 400,
                    ResponseMessage = "Invalid credentials."
                };



            var jwtSettings = new JWTSettings();
            _config.GetSection("Jwt").Bind(jwtSettings);


            var token = JwtTokenHelper.GenerateToken(
     user.UserName ?? "",
     user.Email,
     user.Id,
     user.Role,
     jwtSettings
 );


            var response = new LoginResponse
            {
                Id = user.Id.ToString(),
                Username = user.UserName ?? "",
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                Token = token
            };


            return ResponseData.GetSuccessResponse(response, "Login successful.");
        }
        catch (Exception ex)
        {
            return ResponseData.ErrorResponse("Login failed", ex.Message);
        }
    }


}
