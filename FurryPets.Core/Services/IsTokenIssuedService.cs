using FurryPets.Core.Interfaces;

namespace FurryPets.Core.Services;

public class IsTokenIssuedService : IIsTokenIssuedService
{
    private readonly IUserRepository _userRepository;

    public IsTokenIssuedService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> IsTokenIssuedAsync(string userId)
    {
        var user = await _userRepository.FindByIdAsync(userId);

        if (user is null)
        {
            return false;
        }

        var result = user.IsAuthTokenIssued;

        return result;
    }
}