using FurryPets.Core.Dto;
using FurryPets.Infrastructure.Entities;
using AutoMapper;

namespace FurryPets.Infrastructure.Mappers;
public class CalendarNoteMapper : Profile
{
    public CalendarNoteMapper()
    {
        CreateMap<CalendarNote, CalendarNoteDto>();
    }
}
