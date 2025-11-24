using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPIActors.Models;


namespace WebAPIActors.Helper
{
    public static class JwtHelper
    {
        private static string _jwtKey;
        private static string _jwtIssue;
        private static string _jwtAudience;


        public static void SetJwtConfig(IConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (string.IsNullOrEmpty(config["Jwt:Key"]) ||
                string.IsNullOrEmpty(config["Jwt:Issuer"]) ||
                string.IsNullOrEmpty(config["Jwt:Audience"]))
            {
                throw new ArgumentException("Configurazione JWT non valida");
            }
            _jwtKey = config["Jwt:Key"];
            _jwtIssue = config["Jwt:Issuer"];
            _jwtAudience = config["Jwt:Audience"];
        }

        public static string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("username", user.Username)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtIssue,
                audience: _jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(2),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
