using AutoMapper;
using FurryPets.Core.Dto;
using FurryPets.Core.Enumerations;
using FurryPets.Infrastructure.Entities;

namespace FurryPets.Infrastructure.Mappers;
public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserDto>()
            .ForMember(userDto => userDto.UserRole,
                memberConfigurationExpression => memberConfigurationExpression.MapFrom(user => Enum.Parse(typeof(RoleType), user.Discriminator)));
    }
}
