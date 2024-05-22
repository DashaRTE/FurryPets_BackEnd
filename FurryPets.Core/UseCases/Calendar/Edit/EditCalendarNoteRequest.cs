namespace FurryPets.Core.UseCases;

public record EditCalendarNoteRequest(Guid CalendarNoteId, string? Reason, string? Note, DateOnly? Date, TimeOnly? Time);