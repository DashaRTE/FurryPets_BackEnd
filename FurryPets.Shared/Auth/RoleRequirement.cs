using Microsoft.AspNetCore.Authorization;

namespace FurryPets.Shared.Auth;

public class RoleRequirement : IAuthorizationRequirement
{
    public RoleRequirement(string roles)
    {
        Roles = roles;
    }

    public string Roles { get; }
}
