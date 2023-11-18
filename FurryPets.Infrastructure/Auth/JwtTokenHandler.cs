using System.IdentityModel.Tokens.Jwt;
using FurryPets.Infrastructure.Interfaces;

namespace FurryPets.Infrastructure.Auth;

public sealed class JwtTokenHandler : IJwtTokenHandler
{
	private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

	public JwtTokenHandler()
	{
		_jwtSecurityTokenHandler ??= new();
	}

	public string WriteToken(JwtSecurityToken jwt) => _jwtSecurityTokenHandler.WriteToken(jwt);
}