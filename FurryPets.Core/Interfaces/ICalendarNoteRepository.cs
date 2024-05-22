using FurryPets.Core.Dto;

namespace FurryPets.Core.Interfaces;
public interface ICalendarNoteRepository
{
    Task<IList<CalendarNoteDto?>> GetCalendarNotesAsync(string userId, DateOnly date);
    Task<CalendarNoteDto?> FindByIdAsync(Guid calendarNoteId);
    Task<CalendarNoteDto> CreateCalendarNoteAsync(string userId, string? reason, string? note, DateOnly? date, TimeOnly? time);

    Task<CalendarNoteDto?> EditCalendarNoteAsync(Guid calendarNoteId, string? reason, string? note, DateOnly? date,
        TimeOnly? time);
    Task CommitAsync();
}
