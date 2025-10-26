namespace ProjectManagementSystem.Application.MediaR.Dtos.AuthDtos
{
    public class RegisterUserRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
