using System.IdentityModel.Tokens.Jwt;

namespace FurryPets.Infrastructure.Interfaces;

public interface IJwtTokenHandler
{
	string WriteToken(JwtSecurityToken jwt);
}