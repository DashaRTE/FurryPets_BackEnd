namespace FurryPets.Core.UseCases;
public record GetCalendarNotesResponse(Guid CalendarNoteId, string? Reason, string? Note, DateOnly? Date, TimeOnly? Time);