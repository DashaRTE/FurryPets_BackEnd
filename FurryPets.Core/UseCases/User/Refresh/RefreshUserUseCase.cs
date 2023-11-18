using System.Net;
using System.Security.Claims;
using FurryPets.Core.Enumerations;
using FurryPets.Core.Interfaces;
using FurryPets.Core.Responses;

namespace FurryPets.Core.UseCases;
public class RefreshUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtFactory _jwtFactory;

    public RefreshUserUseCase(IUserRepository userRepository, IJwtFactory jwtFactory)
    {
        _userRepository = userRepository;
        _jwtFactory = jwtFactory;
    }
    public async Task<ResultResponse<AccessTokenResponse>> HandleAsync(RefreshUserRequest message)
    {
        if (!_jwtFactory.Validate(message.Token))
        {
            return new(){StatusCode = HttpStatusCode.Forbidden};
        }

        var claims = _jwtFactory.Parse(message.Token);

        var userId = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return new() { StatusCode = HttpStatusCode.Forbidden };
        }

        var user = await _userRepository.FindByIdAsync(userId);

        if (user?.IsAuthTokenIssued != true)
        {
            return new() { StatusCode = HttpStatusCode.Unauthorized, Message = "Invalid token" };
        }

        await _userRepository.CommitAsync();

        var userClaims = await _userRepository.GetClaimsAsync(user.Id);

        if (userClaims is null)
        {
            return new() { StatusCode = HttpStatusCode.InternalServerError, Message = "Get claims error" };
        }

        var accessToken = _jwtFactory.GenerateEncodedToken(userClaims, TokenType.AccessToken);

        var refreshToken = _jwtFactory.GenerateEncodedToken(
            new List<Claim> { new(ClaimTypes.NameIdentifier, user.Id) }, TokenType.RefreshToken);

        var response = new AccessTokenResponse(accessToken, refreshToken);

        return new() { StatusCode = HttpStatusCode.OK, Data = response};
    }
}
