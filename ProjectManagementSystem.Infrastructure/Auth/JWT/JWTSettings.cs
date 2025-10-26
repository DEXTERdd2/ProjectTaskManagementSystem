namespace GamePlanBackend.Infrastructure.Auth.JWT
{
    public class JWTSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public double AuthTokenDurationInMinutes { get; set; }
        public double RefreshTokenDurationInDays { get; set; }
    }
}
