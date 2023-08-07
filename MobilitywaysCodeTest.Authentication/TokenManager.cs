using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MobilitywaysCodeTest.Authentication.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MobilitywaysCodeTest.Authentication
{
    public class TokenManager : ITokenManager
    {
        private readonly IOptions<AuthenticationOptions> _options;

        public TokenManager(IOptions<AuthenticationOptions> options) { 
            _options = options;
        }

        public string GenerateToken(int userId)
        {

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Value.Secret));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _options.Value.Issuer,
                Audience = _options.Value.Audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenSerialized = tokenHandler.WriteToken(token);
            return tokenSerialized;
        }

        public SecurityToken ValidateToken(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Value.Secret));

            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _options.Value.Issuer,
                ValidAudience = _options.Value.Audience,
                IssuerSigningKey = key
            }, out SecurityToken validatedToken);
            return validatedToken;
        }
    }
}