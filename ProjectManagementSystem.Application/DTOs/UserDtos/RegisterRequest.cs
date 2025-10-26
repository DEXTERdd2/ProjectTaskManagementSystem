

namespace ProjectManagementSystem.Application.DTOs.UserDtos
{
    public class RegisterRequest
    {
        public string? Username { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
