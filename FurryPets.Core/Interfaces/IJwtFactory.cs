using FurryPets.Core.Enumerations;
using System.Security.Claims;

namespace FurryPets.Core.Interfaces;
public interface IJwtFactory
{
    string GenerateEncodedToken(IEnumerable<Claim> claims, TokenType tokenType);
    IEnumerable<Claim> Parse(string token);
    bool Validate(string token);
}
