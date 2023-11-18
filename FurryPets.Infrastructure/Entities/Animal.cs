namespace FurryPets.Infrastructure.Entities;
public class Animal : Entity
{
    public string UserId { get; set; }
    public User User { get; set; }
}
