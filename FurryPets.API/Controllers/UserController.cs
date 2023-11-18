using FurryPets.Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using FurryPets.Core.Responses;
using FurryPets.Shared;
using System.Security.Claims;

namespace FurryPets.API.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/user")]
[Authorize]

public class UserController : ControllerBase
{
    private readonly RegisterUserUseCase _registerUserUseCase;
    private readonly LoginUserUseCase _loginUserUseCase;
    private readonly RefreshUserUseCase _refreshUserUseCase;
    private readonly LogoutUserUseCase _logoutUserUseCase;

    public UserController(RegisterUserUseCase registerUserUseCase, LoginUserUseCase loginUserUseCase, RefreshUserUseCase refreshUserUseCase, LogoutUserUseCase logoutUserUseCase)
    {
        _registerUserUseCase = registerUserUseCase;
        _loginUserUseCase = loginUserUseCase;
        _refreshUserUseCase = refreshUserUseCase;
        _logoutUserUseCase = logoutUserUseCase;
    }

    [HttpPost, Route("register")]
    [ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status201Created)]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync([FromBody, Required] Requests.RegisterUserRequest request)
    {
        var response = await _registerUserUseCase.HandleAsync(new(request.Username, request.Email, request.Password));

        if (string.IsNullOrWhiteSpace(response.Message))
        {
            return new ObjectResult(response.Data) { StatusCode = (int)response.StatusCode };
        }

        return new ObjectResult(response.Message) { StatusCode = (int)response.StatusCode };
    }

    [HttpPost, Route("login")]
    [ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync([FromBody, Required] Requests.LoginUserRequest request)
    {
        var response = await _loginUserUseCase.HandleAsync(new(request.Email, request.Password));

        if (string.IsNullOrWhiteSpace(response.Message))
        {
            return new ObjectResult(response.Data) { StatusCode = (int)response.StatusCode };
        }

        return new ObjectResult(response.Message) { StatusCode = (int)response.StatusCode };
    }

    [HttpPost, Route("logout")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> LogoutAsync()
    {
        var userId = HttpContext.User.Claims.First(static claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        var response = await _logoutUserUseCase.HandleAsync(new(userId));

        if (string.IsNullOrWhiteSpace(response.Message))
        {
            return new ObjectResult(response.Message) { StatusCode = (int)response.StatusCode };
        }

        return new ObjectResult(response.Message) { StatusCode = (int)response.StatusCode };
    }

    [HttpPost, Route("refresh")]
    [ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshAsync([FromBody, Required] Requests.RefreshUserRequest request)
    {
        var response = await _refreshUserUseCase.HandleAsync(new(request.Token));

        if (string.IsNullOrWhiteSpace(response.Message))
        {
            return new ObjectResult(response.Data) { StatusCode = (int)response.StatusCode };
        }

        return new ObjectResult(response.Message) { StatusCode = (int)response.StatusCode };
    }


}
