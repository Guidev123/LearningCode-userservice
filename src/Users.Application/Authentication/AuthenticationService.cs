using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Users.Application.Extensions;
using Users.Domain.Entities;

namespace Users.Application.Authentication
{
    public class AuthenticationService(IOptions<JsonWebTokenData> jwt) : IAuthenticationService
    {
        private readonly JsonWebTokenData _jwt = jwt.Value;

        public string ComputeSha256Hash(string password)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new();

            for (int i = 0; i < bytes.Length; i++) builder.Append(bytes[i].ToString("x2"));

            return builder.ToString();
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var issuer = _jwt.Issuer;
            var audience = _jwt.Audience;
            var key = _jwt.Key;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Email.Address),
                new(ClaimTypes.Email, user.Email.Address),
                new(ClaimTypes.Role, user.UserType.ToString()),
            };

            var token = new JwtSecurityToken(issuer, audience, claims, null, DateTime.Now.AddDays(15), credentials);


            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
