


namespace ProjectManagementSystem.Application.DTOs.UserDtos
{
    public class LoginResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
