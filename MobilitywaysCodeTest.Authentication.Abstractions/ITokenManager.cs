using Microsoft.IdentityModel.Tokens;

namespace MobilitywaysCodeTest.Authentication.Abstractions
{
    public interface ITokenManager
    {
        string GenerateToken(int userId);
        SecurityToken ValidateToken(string token);
    }
}