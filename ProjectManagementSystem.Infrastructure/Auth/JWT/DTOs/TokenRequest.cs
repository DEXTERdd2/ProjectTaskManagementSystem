namespace GamePlanBackend.Infrastructure.Auth.JWT.DTOS
{
    public class TokenRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}