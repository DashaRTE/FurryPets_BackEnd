using FurryPets.Core.Interfaces;
using FurryPets.Core.Services;
using FurryPets.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FurryPets.Di.DiManagers;

public static class ServicesDi
{
    public static IServiceCollection AddServices(this IServiceCollection services) =>
        services
            .AddScoped<IIsTokenIssuedService, IsTokenIssuedService>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ICalendarNoteRepository, CalendarNoteRepository>();
}