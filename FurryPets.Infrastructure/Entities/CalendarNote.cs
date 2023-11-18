namespace FurryPets.Infrastructure.Entities;
public class CalendarNote : Entity
{
    public string? Reason { get; set; }
    public string? Note { get; set; }
    public DateOnly? Date { get; set; }
    public TimeOnly? Time { get; set; }

    public string UserId  { get; set;}
    public User User { get; set; }
}
