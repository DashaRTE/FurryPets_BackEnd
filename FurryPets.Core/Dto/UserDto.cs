using FurryPets.Core.Enumerations;

namespace FurryPets.Core.Dto;
public record UserDto
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public RoleType UserRole { get; set; }
    public bool IsAuthTokenIssued { get; set; }
    public DateTime CreationDate { get; set; }

    public IList<AnimalDto> Animals { get; set; }
}
