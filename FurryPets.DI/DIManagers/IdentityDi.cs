using FurryPets.Infrastructure;
using FurryPets.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FurryPets.Di.DiManagers;

public static class IdentityDi
{
	public static IServiceCollection AddIdentity(this IServiceCollection services)
	{
		services.AddIdentityCore<User>(identityOptions =>
			{
				identityOptions.User.AllowedUserNameCharacters = string.Empty;
				identityOptions.Lockout.MaxFailedAccessAttempts = 5;
				identityOptions.Password.RequireDigit = true;
				identityOptions.Password.RequireLowercase = true;
				identityOptions.Password.RequireUppercase = true;
				identityOptions.Password.RequireNonAlphanumeric = false;
				identityOptions.Password.RequiredLength = 8;
				identityOptions.User.RequireUniqueEmail = true;
				identityOptions.Tokens.PasswordResetTokenProvider = "TokenProvider";
			})
			.AddUserStore<UserStore<User, IdentityRole, DataContext, string>>()
			.AddSignInManager<SignInManager<User>>()
			.AddDefaultTokenProviders()
			.AddTokenProvider<DataProtectorTokenProvider<User>>("TokenProvider");

		return services;
	}
}