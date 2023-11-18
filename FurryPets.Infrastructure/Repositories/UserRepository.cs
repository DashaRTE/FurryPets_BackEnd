using System.Security.Claims;
using AutoMapper;
using FurryPets.Core.Dto;
using FurryPets.Core.Enumerations;
using FurryPets.Core.Interfaces;
using FurryPets.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FurryPets.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;

    public UserRepository(
        DataContext context,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    public Task<bool> AnyAsync() => _context.Users.AnyAsync();

    public Task<bool> AnyEmailAsync(string email) =>
        _context.Users.AnyAsync(user => user.Email == email);

    public async Task<bool> AnyUsernameAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
        {
            return false;
        }

        return true;
    }

    public async Task<UserDto?> FindByEmailAsync(string email) => 
        _mapper.Map<User?, UserDto?>(await _userManager.FindByEmailAsync(email));

    public async Task<UserDto?> FindByIdAsync(string userId) =>
        _mapper.Map<User?, UserDto?>(await _userManager.FindByIdAsync(userId));

    public async Task<bool> SetUserEmailAsync(string userId, string email)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return false;
        }

        var result = await _userManager.SetEmailAsync(user, email);

        return result.Succeeded;
    }

    public async Task<IdentityResult> AddClaimsAsync(string userId, IEnumerable<Claim> claims)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }

        return await _userManager.AddClaimsAsync(user, claims);
    }

    public async Task<IList<Claim>?> GetClaimsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return null;
        }

        return await _signInManager.UserManager.GetClaimsAsync(user);
    }

    public async Task<SignInResult> CheckPasswordSignInAsync(string userId, string password, bool lockoutOnFailure)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return SignInResult.Failed;
        }

        return await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
    }

    public async Task SetAccessTokenIssueStateAsync(string userId, bool state)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is not null)
        {
            user.IsAuthTokenIssued = state;
            _context.Entry(user).State = EntityState.Modified;
        }
    }

    public Task<IdentityResult> CreateAsync(string username, string email, string password)
    {
        var user = new User
        {
            UserName = username,
            Email = email,
            Discriminator = RoleType.User.ToString(),
            EmailConfirmed = true
        };

        return _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        return result;
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return false;
        }

        var result = await _userManager.DeleteAsync(user);

        return result.Succeeded;
    }

    public async Task<string?> GetUserEmailAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return null;
        }

        return await _userManager.GetEmailAsync(user);
    }

    public async Task<string?> GetAuthenticatorKeyAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return null;
        }

        return await _userManager.GetAuthenticatorKeyAsync(user);
    }

    public async Task<IdentityResult> ResetAuthenticatorKeyAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }

        return await _userManager.ResetAuthenticatorKeyAsync(user);
    }

    public string GetTokenProvider() => _userManager.Options.Tokens.AuthenticatorTokenProvider;

    public async Task CommitAsync() => await _context.SaveChangesAsync();
}
