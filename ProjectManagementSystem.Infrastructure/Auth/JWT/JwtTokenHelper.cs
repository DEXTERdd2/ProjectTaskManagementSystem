using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace GamePlanBackend.Infrastructure.Auth.JWT
{
    public class JwtTokenHelper
    {

        public static string GenerateToken(
            string name,
            string email,
            int userId,
            string role,
            JWTSettings settings
        )
        {
            if (string.IsNullOrWhiteSpace(settings.SecretKey) || Encoding.UTF8.GetByteCount(settings.SecretKey) < 16)
                throw new ArgumentException("Secret key is invalid. Minimum 16 characters.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, name ?? ""),
                new Claim(ClaimTypes.Email, email ?? ""),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role ?? "User"),
                new Claim("AppName", "ProjectManagementSystem")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = settings.Issuer,
                Audience = settings.Audience,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(settings.AuthTokenDurationInMinutes),
                SigningCredentials = creds
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        // Decode JWT token into dictionary
        public static IDictionary<string, string> DecodeToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty.");

            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token))
                throw new ArgumentException("Invalid JWT token format.");

            var jwtToken = handler.ReadToken(token) as JwtSecurityToken
                ?? throw new ArgumentException("Invalid token.");

            return jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
        }
    }
}
