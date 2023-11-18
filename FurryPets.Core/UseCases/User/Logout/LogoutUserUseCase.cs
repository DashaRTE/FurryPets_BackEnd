using System.Net;
using FurryPets.Core.Interfaces;
using FurryPets.Core.Responses;

namespace FurryPets.Core.UseCases;
public class LogoutUserUseCase
{
    private readonly IUserRepository _userRepository;

    public LogoutUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ResultResponse> HandleAsync(LogoutUserRequest message)
    {
        await _userRepository.SetAccessTokenIssueStateAsync(message.UserId, false);

        await _userRepository.CommitAsync();

        return new() { StatusCode = HttpStatusCode.OK, Message = "Success" };
    }
}
