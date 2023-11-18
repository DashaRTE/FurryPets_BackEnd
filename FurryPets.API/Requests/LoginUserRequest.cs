using System.ComponentModel.DataAnnotations;

namespace FurryPets.API.Requests;

public record LoginUserRequest([Required, EmailAddress] string Email,
    [Required, DataType(DataType.Password)] string Password);
