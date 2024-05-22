using AutoMapper;
using FurryPets.Infrastructure.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace FurryPets.Di.DiManagers;

public static class MapperDi
{
	public static IServiceCollection AddMapper(this IServiceCollection services)
	{
		var mappingConfig = new MapperConfiguration(static mapperConfigurationExpression =>
		{
            mapperConfigurationExpression.AddProfile(new CalendarNoteMapper());
		});

		var mapper = mappingConfig.CreateMapper();
		services.AddSingleton(mapper);

		return services;
	}
}