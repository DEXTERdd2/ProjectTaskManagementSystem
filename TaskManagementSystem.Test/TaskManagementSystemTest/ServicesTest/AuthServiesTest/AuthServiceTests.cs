using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectManagementSystem.Application.DTOs.UserDtos;
using ProjectManagementSystem.Infrastructure.Data;

public class AuthServiceTests : IAsyncLifetime
{
    private AppDbContext _context;
    private IConfiguration _config;
    private AuthService _service;

    // This runs before each test
    public async Task InitializeAsync()
    {
        // Setup fresh in-memory DB
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);

        // Setup configuration for JWT
        var inMemorySettings = new Dictionary<string, string>
        {
            {"Jwt:SecretKey", "SuperSecretKey12345"},
            {"Jwt:Issuer", "ProjectManagementSystem"},
            {"Jwt:Audience", "ProjectManagementSystemUsers"},
            {"Jwt:AuthTokenDurationInMinutes", "1440"}
        };
        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        // Initialize service
        _service = new AuthService(_context, _config);

        await Task.CompletedTask;
    }

    // This runs after each test
    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.DisposeAsync();
    }

    [Fact]
    public async Task RegisterAsync_ShouldRegisterUser_WhenValidRequest()
    {
        var request = new RegisterRequest
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "Password123!"
        };

        var result = await _service.RegisterAsync(request);

        Assert.Equal(200, result.statusCode);
        Assert.Equal("User registered successfully.", result.ResponseMessage);
        Assert.Single(_context.Tb_Users);
        var user = _context.Tb_Users.First();
        Assert.Equal("test@example.com", user.Email);
        Assert.Equal("testuser", user.UserName);
        Assert.True(BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash));
    }

    [Fact]
    public async Task RegisterAsync_ShouldFail_WhenEmailAlreadyExists()
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("Password123!");
        _context.Tb_Users.Add(new Tb_User
        {
            Email = "test@example.com",
            UserName = "existinguser",
            PasswordHash = passwordHash
        });
        await _context.SaveChangesAsync();

        var request = new RegisterRequest
        {
            Username = "newuser",
            Email = "test@example.com",
            Password = "Password123!"
        };

        var result = await _service.RegisterAsync(request);

        Assert.Equal(400, result.statusCode);
        Assert.Equal("User already exists with this email.", result.ResponseMessage);
    }









    [Fact]
    public async Task LoginAsync_ShouldFail_WhenInvalidPassword()
    {
        var correctPassword = "Password123!";
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(correctPassword);

        _context.Tb_Users.Add(new Tb_User
        {
            Email = "test@example.com",
            UserName = "testuser",
            PasswordHash = passwordHash
        });
        await _context.SaveChangesAsync();

        var request = new LoginRequest
        {
            Email = "test@example.com",
            Password = "WrongPassword!"
        };

        var result = await _service.LoginAsync(request);

        Assert.Equal(400, result.statusCode);
        Assert.Equal("Invalid credentials.", result.ResponseMessage);
    }
}
