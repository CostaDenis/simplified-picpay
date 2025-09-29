using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using simplified_picpay.Services.Abstractions;

namespace simplified_picpay.Services
{
    public class TokenService(IConfiguration configuration) : ITokenService
    {
        private readonly IConfiguration _configuration = configuration;

        public Guid GetAccounId(HttpContext http)
        {
            var claim = http.User.FindFirst(ClaimTypes.NameIdentifier);

            return Guid.Parse(claim!.Value);
        }

        public string GenerateTokenJwt(Guid id, string email, string accountType)
        {
            var key = _configuration["Jwt"];

            if (string.IsNullOrEmpty(key))
                throw new Exception("Chave Jwt não encontrada!");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, accountType)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}