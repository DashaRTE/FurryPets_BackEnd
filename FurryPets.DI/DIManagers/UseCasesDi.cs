using FurryPets.Core.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace FurryPets.Di.DiManagers;

public static class UseCasesDi
{
	public static IServiceCollection AddUseCases(this IServiceCollection services) =>
		services
            .AddScoped<GetCalendarNotesUseCase>()
            .AddScoped<CreateCalendarNoteUseCase>()
            .AddScoped<EditCalendarNoteUseCase>();
}