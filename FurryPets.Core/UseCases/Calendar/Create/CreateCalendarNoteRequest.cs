namespace FurryPets.Core.UseCases;

public record CreateCalendarNoteRequest(string UserId, string? Reason, string? Note, DateOnly? Date, TimeOnly? Time);
