using FurryPets.Core.Dto;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FurryPets.Core.Interfaces;
public interface IUserRepository
{
    Task<bool> AnyAsync();
    Task<bool> AnyEmailAsync(string email);
    Task<bool> AnyUsernameAsync(string username);
    Task<UserDto?> FindByEmailAsync(string email);
    Task<UserDto?> FindByIdAsync(string userId);
    Task<bool> SetUserEmailAsync(string userId, string email);
    Task<IdentityResult> AddClaimsAsync(string userId, IEnumerable<Claim> claims);
    Task<IList<Claim>?> GetClaimsAsync(string userId);
    Task<SignInResult> CheckPasswordSignInAsync(string userId, string password, bool lockoutOnFailure);
    Task SetAccessTokenIssueStateAsync(string userId, bool state);
    Task<IdentityResult> CreateAsync(string username, string email, string password);
    Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    Task<bool> DeleteUserAsync(string userId);
    Task<string?> GetUserEmailAsync(string userId);
    Task<string?> GetAuthenticatorKeyAsync(string userId);
    Task<IdentityResult> ResetAuthenticatorKeyAsync(string userId);
    string GetTokenProvider();
    Task CommitAsync();
}
