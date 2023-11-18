using Microsoft.AspNetCore.Identity;

namespace FurryPets.Infrastructure.Entities;

public class User : IdentityUser
{
    public User()
    {
        CreationDate = DateTime.UtcNow;
    }
   
    public bool IsAuthTokenIssued { get; set; }
    public string Discriminator { get; set; }
    public DateTime CreationDate { get; set; }

    public ICollection<Animal> Animals { get; }
    public ICollection<CalendarNote> CalendarNotes { get; }
}
