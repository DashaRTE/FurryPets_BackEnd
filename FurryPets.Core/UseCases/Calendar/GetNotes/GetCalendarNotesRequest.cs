namespace FurryPets.Core.UseCases;
public record GetCalendarNotesRequest(string UserId, DateOnly Date);