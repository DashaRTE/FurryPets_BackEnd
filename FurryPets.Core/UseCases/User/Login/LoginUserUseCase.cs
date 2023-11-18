using FurryPets.Core.Enumerations;
using FurryPets.Core.Interfaces;
using FurryPets.Core.Responses;
using System.Net;
using System.Security.Claims;

namespace FurryPets.Core.UseCases;

public class LoginUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtFactory _jwtFactory;

    public LoginUserUseCase(IUserRepository userRepository, IJwtFactory jwtFactory)
    {
        _userRepository = userRepository;
        _jwtFactory = jwtFactory;
    }

    public async Task<ResultResponse<AccessTokenResponse>> HandleAsync(LoginUserRequest request)
    {
        var user = await _userRepository.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return new() { StatusCode = HttpStatusCode.Forbidden, Message = "Your email or password is incorrect" };
        }

        var result = await _userRepository.CheckPasswordSignInAsync(user.Id, request.Password, false);

        if (!result.Succeeded)
        {
            return new() { StatusCode = HttpStatusCode.Forbidden, Message = "Your email or password is incorrect" };
        }

        var userClaims = (await _userRepository.GetClaimsAsync(user.Id))?.ToList();

        if (userClaims is null)
        {
            return new() { StatusCode = HttpStatusCode.InternalServerError, Message = "Get claims error" };
        }

        if (userClaims.All(static claim => claim.Type != ClaimTypes.Role))
        {
            var newUserClaims = new[] { new Claim(ClaimTypes.NameIdentifier, user.Id), new Claim(ClaimTypes.Role, user.UserRole.ToString()) };

            await _userRepository.AddClaimsAsync(user.Id, newUserClaims);

            userClaims.AddRange(newUserClaims);
        }

        userClaims = userClaims.Where(claim => !(claim.Type == ClaimTypes.Role && claim.Value != user.UserRole.ToString())).ToList();

        var accessToken = _jwtFactory.GenerateEncodedToken(userClaims, TokenType.AccessToken);

        var refreshToken = _jwtFactory.GenerateEncodedToken(new List<Claim> { new(ClaimTypes.NameIdentifier, user.Id) },
            TokenType.RefreshToken);

        var response = new AccessTokenResponse(accessToken, refreshToken);

        await _userRepository.SetAccessTokenIssueStateAsync(user.Id, true);

        await _userRepository.CommitAsync();

        return new() { StatusCode = HttpStatusCode.OK, Data = response };
    }
}