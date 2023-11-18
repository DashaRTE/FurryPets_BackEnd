namespace FurryPets.Core.Dto;
public record CalendarNoteDto : EntityDto
{
    public string? Reason { get; set; }
    public string? Note { get; set; }
    public DateOnly? Date { get; set; }
    public TimeOnly? Time { get; set; }
    public string UserId { get; set; }
}
