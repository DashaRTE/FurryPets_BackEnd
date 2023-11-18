using System.ComponentModel.DataAnnotations;

namespace FurryPets.Infrastructure.Entities;
public abstract class Entity
{
    [Key]
    public Guid Id { get; set; }
}
