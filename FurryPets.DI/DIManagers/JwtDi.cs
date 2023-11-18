using FurryPets.Core.Interfaces;
using FurryPets.Infrastructure.Auth;
using FurryPets.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FurryPets.Di.DiManagers;

public static class JwtDi
{
    public static IServiceCollection AddJwt(this IServiceCollection services) =>
        services.AddScoped<IJwtFactory, JwtFactory>()
            .AddScoped<IJwtTokenHandler, JwtTokenHandler>();
}