using AutoMapper;
using FurryPets.Core.Dto;
using FurryPets.Infrastructure.Entities;

namespace FurryPets.Infrastructure.Mappers;
public class AnimalMapper : Profile
{
    public AnimalMapper()
    {
        CreateMap<Animal, AnimalDto>();
    }
}
