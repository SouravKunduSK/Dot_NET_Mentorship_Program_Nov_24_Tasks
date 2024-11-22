using Basic_Authentication_System.Models.Database;
using Microsoft.CodeAnalysis.Elfie.Model.Strings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Basic_Authentication_System.Helpers
{
    public class JWTServiceHelper
    {
        private readonly IConfiguration _configuration;
        public JWTServiceHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GenerateTokenAsync(User user)
        {
            var claims = new[]
            {
                
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:ExpirationMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
